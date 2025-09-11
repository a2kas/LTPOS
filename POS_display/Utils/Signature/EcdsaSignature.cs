using iTextSharp.text.pdf.security;
using System;
using System.Security.Cryptography;

namespace POS_display.Utils.Signature
{
    public class EcdsaSignature : IExternalSignature
    {
        private readonly string _encryptionAlgorithm;
        private readonly string _hashAlgorithm;
        private readonly ECDsa _pk;
        private const string EcdsaEncryptionAlgorithm = "ECDSA";

        public EcdsaSignature(ECDsa pk, string hashAlgorithm)
        {
            _pk = pk ?? throw new ArgumentNullException(nameof(pk), "ECDSA private key cannot be null.");
            _hashAlgorithm = DigestAlgorithms.GetDigest(DigestAlgorithms.GetAllowedDigests(hashAlgorithm));
            _encryptionAlgorithm = EcdsaEncryptionAlgorithm;
        }

        public virtual string GetEncryptionAlgorithm()
        {
            return _encryptionAlgorithm;
        }

        public virtual string GetHashAlgorithm()
        {
            return _hashAlgorithm;
        }

        public virtual byte[] Sign(byte[] message)
        {
            return _pk.SignData(message, new HashAlgorithmName(_hashAlgorithm));
        }
    }
}
