using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace POS_display
{
    public partial class TEST : Form
    {
        private Items.posh PoshItem;
        private bool formWaiting = false;

        #region Callbacks
        private void GetClassifier_cb(bool success, string resultXML, XElement inParams)
        {
            // we are different thread!
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (success)
                {
                    try
                    {
                        XElement root = XElement.Parse(resultXML);
                        var error = from el in root.Descendants("t")
                                    select new
                                    {
                                        txt = (string)el,
                                        total = (int)el.Element("Total")
                                    };

                        if (error.First().total <= 0)
                            throw new Exception("Klasifikatorius nerastas!");

                        var classifiers = (from el in root.Descendants("t").Elements("Item")
                                           select new
                                           {
                                               ValidFrom = XmlConvert.ToDateTime((string)el.Element("ValidFrom"), XmlDateTimeSerializationMode.Local),
                                               SysModifyTime = (string)el.Element("SysModifyTime"),
                                               ClassCode = (string)el.Element("ClassCode"),
                                               DisplayCode = (string)el.Element("DisplayCode"),
                                               DisplayName = (string)el.Element("DisplayName"),
                                               TypeCode = (string)el.Element("TypeCode"),
                                               ValidTo = (string)el.Element("ValidTo") == "" ? new DateTime(3000, 1, 1) : XmlConvert.ToDateTime((string)el.Element("ValidTo"), XmlDateTimeSerializationMode.Local)
                                           }).Where(x => x.ValidFrom <= DateTime.Now && x.ValidTo > DateTime.Now);
                        gvRes.DataSource = classifiers.ToList();
                    }
                    catch (Exception ex)
                    {
                        helpers.alert(Enumerator.alert.warning, ex.Message);
                    }
                }
            }));
        }


        #endregion
        public TEST(Items.posh posh_item)
        {
            InitializeComponent();
            PoshItem = posh_item;
            //default
            tbProductCodeScheme.Text = "gtin";
            tbProductCode.Text = "05000456013482";
            tbSerialNumber.Text = "0000000001";
            tbBatchId.Text = "00001";
            tbExpiryDate.Text = "201200";
        }

        private void TEST_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.formWaiting == true)
                e.Cancel = true;
        }

        private void form_wait(bool wait)
        {
            if (wait == true)
            {
                this.UseWaitCursor = true;
                this.Cursor = Cursors.WaitCursor;
                this.formWaiting = true;
            }
            else
            {
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                this.formWaiting = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnErecClassificationList_Click(object sender, EventArgs e)
        {
            if (formWaiting == true)
                return;
            form_wait(true);
            //Session.eRecipeUtils.GetClassifier(tbValue.Text == "" ? "classification-list" : tbValue.Text, "", GetClassifier_cb);
            form_wait(false);
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                await Program.Display1.SubmitBarcode(new Models.Barcode { BarcodeStr = tbProductCode.Text });
            }
            catch (Exception ex)
            {
                helpers.alert(Enumerator.alert.error, ex.Message);
            }
        }
    }
}
