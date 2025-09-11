using AutoMapper;
using POS_display.Display.Views;
using POS_display.Repository.Discount;
using POS_display.Repository.Loyalty;
using POS_display.Repository.NBO;
using POS_display.Repository.PersonalPharmacist;
using POS_display.Repository.Pos;
using POS_display.Repository.Price;
using POS_display.Repository.Recipe;
using POS_display.Repository.SalesOrder;
using POS_display.Views.Display;
using System;
using Tamroutilities.Client;

namespace POS_display.Presenters.Display
{
    public class Display2Presenter : BasePresenter, IDisplay2Presenter
    {
        private IDisplay2View _view;
        private readonly ITamroClient _tamroClient;

        public Display2Presenter(IDisplay2View view, ITamroClient tamroClient)
        {
            _view = view;
            _tamroClient = tamroClient ?? throw new ArgumentNullException();
        }
    }
}
