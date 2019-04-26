using System;
using System.Collections.Generic;
using System.Text;
using BarcodeInspection.Models.Outbound;

namespace BarcodeInspection.ViewModels.Outbound
{
    public class LOBSM030ViewModel : ViewModelBase
    {
        private LOBSM020Model lobsm020Model;
        private DateTime _deptid1;
        private bool isTranToggle;

        public LOBSM030ViewModel(LOBSM020Model lobsm020Model, DateTime deptid1, bool isTranToggle)
        {
            this.lobsm020Model = lobsm020Model;
            _deptid1 = deptid1;
            this.isTranToggle = isTranToggle;
        }
    }
}
