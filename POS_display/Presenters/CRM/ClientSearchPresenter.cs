using AutoMapper;
using ExternalServices.CareCloudREST.Models.CortexModels.Cards;
using ExternalServices.CareCloudREST.Models.CortexModels.Customers;
using Microsoft.Extensions.DependencyInjection;
using POS_display.Models.CRM;
using POS_display.Utils.Logging;
using POS_display.Views.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_display.Presenters.CRM
{
    public class ClientSearchPresenter : BasePresenter, IClientSearchPresenter
    {
        #region Members
        private readonly IClientSearchView _view;
        private const int MaxClients = 15;
        #endregion

        #region Constructor
        public ClientSearchPresenter(IClientSearchView view)
        {
            _view = view;
            ClearForm();
        }
        #endregion

        #region Public methods
        public async Task LoadClients()
        {
            var requestParams = new CustomerRequestParams()
            {
                Count = MaxClients,
                Email = _view.Email.Text,
                Phone = _view.Phone.Text,
                FirstName = _view.ClientName.Text,
                LastName = _view.Surename.Text
            };

            if (DateTime.TryParse(_view.BirthDate.Text, out DateTime birthdate))
                requestParams.BirthDate = birthdate;

            Serilogger.GetLogger().Information($"[Load CRM Clients] Request params: {requestParams.ToJsonString()}");

            List<Cortex.Client.Model.Customer> customers = await Session.CRMRestUtils.GetCustomers(
                requestParams.Count,
                requestParams.Email,
                requestParams.Phone,
                requestParams.FirstName,
                requestParams.LastName,
                requestParams.BirthDate);

            var mapper = Program.ServiceProvider.GetRequiredService<IMapper>();
            var clients = mapper.Map<List<CRMClientData>>(customers);

            if (clients != null && clients.Count == 0)
            {
                helpers.alert(Enumerator.alert.warning, "Pagal nustatytus kriterijus nepavyko rasti nei vieno egzistuojančio kliento!");
                _view.Clients = new List<CRMClientData>();
            }
            else if (clients.Count > MaxClients)
            {
                helpers.alert(Enumerator.alert.warning, "Nurodykite tikslesnius paieškos kriterijus ir bandykite iš naujo!");
                _view.Clients = new List<CRMClientData>();
            }
            else
            {
                await AttachClientsCardData(clients);
                _view.Clients = clients;
            }
        }

        public void ClearForm()
        {
            _view.Email.Text = string.Empty;
            _view.Phone.Text = string.Empty;
            _view.ClientName.Text = string.Empty;
            _view.Surename.Text = string.Empty;
            _view.BirthDate.Clear();
            _view.Clients = new List<CRMClientData>();
            _view.FocusedClient = null;
            EnableControls();
        }

        public void EnableControls() 
        {
            _view.ConfirmButton.Enabled = _view.FocusedClient != null;
        }

        public string ValidateSearchData()
        {
            return new CRMClientData
            {
                Name = _view.ClientName.Text,
                Surename = _view.Surename.Text,
                Phone = _view.Phone.Text,
                Email = _view.Email.Text,
                BirthDate = _view.BirthDate.MaskFull ? _view.BirthDate.Text : string.Empty
            }.Validate();
        }

        private async Task AttachClientsCardData(List<CRMClientData> clients) 
        {
            foreach (var client in clients) 
            {
                client.CardNumber = await Session.CRMRestUtils.GetCardByCustomerId(client.ID);
            }
        }
        #endregion
    }
}
