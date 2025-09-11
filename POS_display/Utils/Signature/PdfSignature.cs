using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Security;
using POS_display.Exceptions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace POS_display.Utils.Signature
{
    [XmlRoot(ElementName = "t")]
    public class SignatureParams
    {
        public string CertificateTypeOid { get; set; }
        public string PersonalCode { get; set; }
        public string Pin { get; set; }
        public string Reason { get; set; }
        public string Location { get; set; }
        public string SignatureCreator { get; set; }
        public string FirstLineText { get; set; }
        public string DateTitleText { get; set; }
        public string TimeTitleText { get; set; }
        public string DateFormat { get; set; }
        public string TimeFormat { get; set; }
        public string FontPath { get; set; }
        //public string FontBoldPath { get; set; }
        public string LogoPath { get; set; }
        public int RectLlx { get; set; }
        public int RectLly { get; set; }
        public int RectUrx { get; set; }
        public int RectUry { get; set; }
        [XmlIgnoreAttribute]
        public IExternalSignature ExternalSignature { get; set; }
        public bool? _LocalTest { get; set; }

        public SignatureParams()
        {
            CertificateTypeOid = "1.3.6.1.4.1.311.10.3.12"; //paieška pagal sertifikato tipą, Document Signing (1.3.6.1.4.1.311.10.3.12)
            Reason = "Vaisto ar MPP išdavimas"; //pdf signature property
            SignatureCreator = "eReceptas"; //pdf signature property
            FirstLineText = "El. parašu pasirašė:"; //tekstas parašo formavimui
            DateTitleText = "Data: "; //tekstas parašo formavimui
            TimeTitleText = "Laikas: "; //tekstas parašo formavimui
            DateFormat = "yyyy.MM.dd"; //datos formatas parašo formavimui
            TimeFormat = "HH:mm:ss"; //laiko formatas parašo formavimui
            FontPath = "Resources\\eRecipe\\DejaVuSansCondensed.ttf"; // kelias iki šrifto @"C:\Windows\Fonts\micross.ttf", //@"%SYSTEMROOT%\Fonts\arial.ttf",
            //FontBoldPath = "DejaVuSansCondensed-Bold.ttf";

            Pin = null; //sertifikato pin. Jei nenurodytas, rodo langelį suvesti pin
            PersonalCode = null; //ieško sertifikato pagal a.k. Jei nenurodytas, duoda pasirinkti sertifikatą

            LogoPath = null; //kelias iki įmones logotipo
            Location = ""; //pdf signature property, SveidraID ?

            //pasirašymo kvadratas, atskaita nuo paskutinio puslapio kairės apačios
            RectLlx = 20;   //rectangle lower left x point
            RectLly = 15;   //rectangle lower left y point
            RectUrx = 290;  //rectangle upper right x point
            RectUry = 65;   //rectangle upper right y point
        }
    }

    public class PdfSignature : IDisposable
    {
        public SignatureParams Parameters;

        private PdfReader _pdfReader;

        private MemoryStream _memStream;

        private PdfStamper _pdfStamper;

        public PdfSignature()
        {
            this.Parameters = new SignatureParams();
        }

        public PdfSignature(SignatureParams p)
        {
            this.Parameters = p;
        }

        public void Dispose()
        {
            try
            {
                _pdfReader?.Dispose();
                _memStream?.Dispose();
                _pdfStamper?.Dispose();
            }
            catch 
            { 
            }
        }

        public byte[] SignInMemory(byte[] unsignedPdf)
        {
            //Sign with certificate selection in the windows certificate store
            X509Store store = new X509Store(StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection selectedCertificates;

            if (!String.IsNullOrWhiteSpace(Parameters.PersonalCode))
            {
                selectedCertificates = store.Certificates.Find(X509FindType.FindByApplicationPolicy, Parameters.CertificateTypeOid, true)
                                                                           .Find(X509FindType.FindBySubjectName, Parameters.PersonalCode, true);
            }
            else
            {
                selectedCertificates = X509Certificate2UI.SelectFromCollection(
                                                                    store.Certificates.Find(X509FindType.FindByApplicationPolicy, Parameters.CertificateTypeOid, true),
                                                                    "Dokumento pasirašymas",
                                                                    "Pasirinkite savo sertifikatą:",
                                                                    X509SelectionFlag.SingleSelection);
            }

            if (selectedCertificates.Count < 1) return null;

            X509Certificate2 cert = selectedCertificates[0];
            //TM: pass personal code
            Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };
            CertificateInfo.X509Name x509name = CertificateInfo.GetSubjectFields(chain[0]);

            if (x509name != null)
            {
                Parameters.PersonalCode = x509name.GetField("SN") ?? "";
            }

            store.Close();
            
            return SignWithCertInMemory(unsignedPdf, cert);

        }

        private Tuple<IExternalSignature, IDisposable> ExtractPrivateKey(X509Certificate2 certificate, string algorithm)
        {
            IExternalSignature signature = null;
            bool foundPk = false;
            int attempt = 0;
            IDisposable key = null;

            while (foundPk is false)
            {
                try
                {
                    switch (attempt)
                    {
                        case 0:
                            AsymmetricAlgorithm pk = certificate.PrivateKey;
                            signature = new X509Certificate2Signature(certificate, algorithm);
                            key = pk;
                            break;
                        case 1:
                            RSA rsa = certificate.GetRSAPrivateKey() ??
                                throw new NotSupportedException("RSA private key is null on the certificate.");
                            signature = new PrivateKeySignature(DotNetUtilities.GetRsaKeyPair(rsa).Private, algorithm);
                            key = rsa;
                            break;
                        case 2:
                            DSA dsa = certificate.GetDSAPrivateKey() ??
                                throw new NotSupportedException("Dsa private key is null on the certificate.");
                            signature = new PrivateKeySignature(DotNetUtilities.GetDsaKeyPair(dsa).Private, algorithm);
                            key = dsa;
                            break;
                        case 3:
                            ECDsa ecdsa = certificate.GetECDsaPrivateKey() ??
                                throw new NotSupportedException("ECDsa private key is null on the certificate.");
                            signature = new EcdsaSignature(ecdsa, algorithm);
                            key = ecdsa;
                            break;
                    }
                    foundPk = true;
                }
                catch (Exception ex)
                {
                    //_logger.LogDebug(e, $"Private key extraction ran into an exception. Attempt (zero-based): {attempt}");
                }

                attempt++;
            }

            if (signature == null)
            {
                throw new NotSupportedException(
                    $"The private key of the certificate could not be retrieved for signing. {certificate.SubjectName}");
            }

            return Tuple.Create(signature, key);
        }

        private byte[] SignWithCertInMemory(byte[] unsignedPdf, X509Certificate2 cert)
        {
            DateTime datetime = DateTime.Now;

            Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };

            if (Parameters.ExternalSignature == null)
            {
                var signature = ExtractPrivateKey(cert, DigestAlgorithms.SHA1);
                Parameters.ExternalSignature = signature.Item1;
            }

            _pdfReader = new PdfReader(unsignedPdf);
            _memStream = new MemoryStream();
            _pdfStamper = PdfStamper.CreateSignature(_pdfReader, _memStream, '\0', null, true);

            try
            {
                #region Signature appearance
                PdfSignatureAppearance signatureAppearance = _pdfStamper.SignatureAppearance;

                signatureAppearance.Reason = Parameters.Reason;
                signatureAppearance.Location = Parameters.Location;

                StringBuilder buf = new StringBuilder();
                buf.Append(Parameters.FirstLineText).AppendLine();
                String name = null;
                CertificateInfo.X509Name x500name = CertificateInfo.GetSubjectFields(chain[0]);
                if (x500name != null)
                {
                    name = x500name.GetField("CN") ?? x500name.GetField("E");
                }
                if (name == null) name = "";

                buf.Append(name).AppendLine();
                buf.Append(Parameters.DateTitleText).Append(datetime.ToString(Parameters.DateFormat)).AppendLine();
                buf.Append(Parameters.TimeTitleText).Append(datetime.ToString(Parameters.TimeFormat));

                string text = buf.ToString();

                BaseFont font = BaseFont.CreateFont(Parameters.FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                signatureAppearance.Layer2Font = new Font(font);
                signatureAppearance.Layer2Text = text;

                if (!String.IsNullOrWhiteSpace(Parameters.LogoPath))
                {
                    signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
                    signatureAppearance.SignatureGraphic = Image.GetInstance(Parameters.LogoPath);
                }
                else
                {
                    signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
                }

                signatureAppearance.SignatureCreator = Parameters.SignatureCreator;

                signatureAppearance.SetVisibleSignature(new Rectangle(Parameters.RectLlx,
                                                  Parameters.RectLly,
                                                  Parameters.RectUrx,
                                                  Parameters.RectUry),
                                    _pdfReader.NumberOfPages, "Signed");
                #endregion

                MakeSignature.SignDetached(signatureAppearance, Parameters.ExternalSignature, chain, null, null, null, 0, CryptoStandard.CMS);

                byte[] signedPdf = _memStream.ToArray();

                return signedPdf;
            }
            catch (Exception ex)
            {
                throw new PdfSignatureException(ex.Message, ex);
            }
        }
    }
}
