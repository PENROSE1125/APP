using BarcodeInspection.Controls;
using BarcodeInspection.Helpers;
using BarcodeInspection.Models.Outbound;
using BarcodeInspection.Services;
using BarcodeInspection.Views.Outbound;
using BarcodeInspection.Views.Common;
using BarcodeInspection.Views;
using DevExpress.Mobile.DataGrid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BarcodeInspection.ViewModels.Outbound
{
    public class LOBSM020ViewModel : ViewModelBase
    {

        private GridControl _gridControl;

        //private ObservableCollection<LIBR020Model> _searchResult = new ObservableCollection<LIBR020Model>();
        private ObservableRangeCollection<LOBSM020Model> _searchResult = new ObservableRangeCollection<LOBSM020Model>();
        private List<LOBSM020Model> _listSearchResult = new List<LOBSM020Model>();
        private string _recvky = string.Empty;
        private string _pltid = string.Empty; //Pallet ID
        private string _tranName = string.Empty;
        private bool _isTranToggle = false;
        private int _rowTotal = 0;
        private int _selectedCount = 0;
        private int _rowHandle = -1;


        private DateTime _deptid1; //입고예정일자, 입고 입력화면으로 전달

        public ICommand GridRowTapCommand { get; }
        public ICommand SwitchToggledCommand { get; }
        public ICommand EntryPltidCompletedCommand { get; }
        public ICommand BarcodeScanCommand { get; }
        public ICommand PullToRefreshCommand { get; }
        public ICommand SearchDetailCommand { get; } //Row선택하세 상세화면으로 이동
        public ICommand SearchCommand { get; }

        //전체 리스트
        private List<string> AllScanBarcode = new List<string>();
        //스캔 완료 바코드
        private List<string> ScanCompletedBarcode = new List<string>();
        //스캔 완료해서 저장처리한 바코드
        private List<string> SaveCompletedBarcode = new List<string>();

        public LOBSM020ViewModel()
        {
            TranName = "미완료";

            GridRowTapCommand = new Command<DevExpress.Mobile.DataGrid.RowTapEventArgs>(async (e) => await GridRowTap(e));
            SwitchToggledCommand = new Command<ToggledEventArgs>(async (e) => await SwitchToggled(e));
            EntryPltidCompletedCommand = new Command(async () => await EntryPltidCompleted());
            BarcodeScanCommand = new Command(async () => await BarcodeScan());
            PullToRefreshCommand = new Command(async () => await PullToRefresh());
            SearchDetailCommand = new Command(async () => await SearchDetail());

        }

        private async Task BarcodeScan()
        {
            if (IsTranToggle)
            {
                return;
            }

            if (!IsEnabled)
            {
                return;
            }

            //화면에 리스트가 없으면 카메라 진입하지 못함.
            if (this.SearchResult.Count == 0)
            {
                return;
            }

            IsBusy = true;
            IsEnabled = false;

            this.Pltid = string.Empty;
            this.Grid.SelectedRowHandle = -1;

            ScanBarcodeView scanBarcode = new ScanBarcodeView(false, false, AllScanBarcode, ScanCompletedBarcode, SaveCompletedBarcode);
            scanBarcode.OnScanCompleted += (List<string> result) =>
            {
                if (result.Count > 0)
                {
                    //LOBL020_ScanCompleted(result);

                    //이렇게 하지 않으면 에러 발생함.
                    //아이폰에서만 에러
                    Device.BeginInvokeOnMainThread(async () =>
                    {
     //                   await LOBSM020_ScanCompleted(result);
                    });
                }
            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                await Application.Current.MainPage.Navigation.PushAsync(scanBarcode);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(scanBarcode);
            }

            IsBusy = false;
            IsEnabled = true;
        }

        public async Task LOBSM020_ScanCompleted(List<string> scanResult)
        {
            //IsBusy = true;
            //IsEnabled = false;
            foreach (var scanItem in scanResult) //카메라 화면에서 받아 온 것 
            {
                foreach (var item in SearchResult) //화면에 있는 데이터
                {
  //                  if (item.Trnuid == scanItem)
  //                  {
                        //해당 라인으로 포커스 이동
                        int rowNum = this.Grid.FindRowByValue("Trnuid", scanItem); //PalletID
                        this.Grid.SelectedRowHandle = rowNum; // 선택
                        this.Grid.ScrollToRow(rowNum); //해당 Row로 이동

                        this.Pltid = this._gridControl.GetCellValue(rowNum, "Trnuid").ToString();

                        //ToDo
                        await SearchDetail();

                        break;
   //                 }
                }
            }

            //LabelCount = SearchResult.Count;
            //IsBusy = false;
            //IsEnabled = true;
        }

        private async Task EntryPltidCompleted()
        {
            if (!string.IsNullOrEmpty(this.Pltid.Trim()))
            {
                await SearchDetail();
            }
        }

        /// <summary>
        /// 상세화면으로 이동
        /// </summary>
        /// <returns></returns>
        private async Task SearchDetail()
        {
            IsEnabled = false;

            if (string.IsNullOrEmpty(this.Recvky))
            {
                IsEnabled = true;
                return;
            }

            if (await VersionCheck.Instance.IsUpdate())
            {
                await VersionCheck.Instance.UpdateCheck();
                IsEnabled = true;

                return;
            }

            if (!string.IsNullOrEmpty(this.Pltid))
            {
                //LIBR020Model libr020Model = new LIBR020Model();

                //IEnumerable<LIBR020Model> resultQuery =
                //from result in this.SearchResult
                //where result.Trnuid == this.Pltid.Trim()
                //select result;

                //foreach (var item in resultQuery)
                //{
                //    libr020Model = item;
                //    break;
                //}

         //       LOBSM020Model lobsm020Model = SearchResult.First<LOBSM020Model>(x => x.Trnuid.Equals(this.Pltid));

                //View화면에서 BindingContext="{x:Reference viewModel} 이렇게 사용할 경우 libr030ViewModel 사용하면 됨.
                //LIBR030ViewModel libr030ViewModel = new LIBR030ViewModel(libr020Model, this._deptid1);

                var NextPagae = new LOBSM030View
                {
                    //완료일 경우 저장 버튼 비활성 한다.
           //         BindingContext = new LOBSM030ViewModel(lobsm020Model, this._deptid1, this.IsTranToggle)
                };

        //        if (!string.IsNullOrEmpty(lobsm020Model.Recvky))
         //       {
                    await Application.Current.MainPage.Navigation.PushAsync(NextPagae);
          //      }
            }

            IsEnabled = true;
        }

        private async Task SwitchToggled(ToggledEventArgs e)
        {
            this.TranName = e.Value ? "완료" : "미완료";

       

            await GetLOBSM020();

            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    await GetLIBR020();
            //});
        }

        public async void Init(DateTime deptid1, string recvky, bool isTranToggle)
        {
            this._deptid1 = deptid1;
            this.Recvky = recvky;

            this.IsTranToggle = isTranToggle;
            //await GetLIBR020();
        }

        private async Task GridRowTap(RowTapEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                this.Pltid = this._gridControl.GetCellValue(e.RowHandle, "Trnuid").ToString();
                //IRowData rowData = this._gridControl.GetRow(0);
                //Console.WriteLine(this._gridControl.GetCellValue(e.RowHandle, "Sebeln"));

                if (this._rowHandle != -1 && this._rowHandle == e.RowHandle)
                {
                    this._selectedCount += 1;
                    if (this._selectedCount == 2)
                    {
                        //Todo
                        //Grid Row Double Click
                        Console.WriteLine(string.Format("Row Double Click : {0}", Pltid));
                        this._selectedCount = 0;

                        //상세 화면이동 LIBR030ViewModel
                        await SearchDetail();
                    }
                }
                else
                {
                    this._rowHandle = e.RowHandle;
                    this._selectedCount = 1;
                }
            }
        }

        private async Task PullToRefresh()
        {
            await GetLOBSM020();
        }

        public async Task GetLOBSM020()
        {
            IsEnabled = false;
     
            //if (!await VersionCheck.Instance.IsNetworkAccess())
            //{
            //    IsEnabled = true;
            //    return;
            //}

            //if (await VersionCheck.Instance.IsUpdate())
            //{
            //    await VersionCheck.Instance.UpdateCheck();
            //    IsEnabled = true;
            //    return;
            //}

            //if (string.IsNullOrEmpty(this.Recvky))
            //{
            //    IsEnabled = true;
            //    return;
            //}

            IsBusy = true;

            this.RowTotal = 0;

            string responseResult = string.Empty;
            string requestParamJson = string.Empty;

            //Clear하면 시간 오래 걸림.
            //this.SearchResult = null;
            //this._listSearchResult = null;
            this.SearchResult.Clear();
            this._listSearchResult.Clear();

            Dictionary<string, string> requestDic = new Dictionary<string, string>();
            if (this.IsTranToggle)
            {
                requestDic.Add("UFN", "{?=call ufn_get_lobsm021(?, ?, ?, ?,?,?)}"); //완료
      
            }
            else
            {
                requestDic.Add("UFN", "{?=call ufn_get_lobsm020(?, ?, ?, ?,?,?)}"); //미완료
      

            }
          
            requestDic.Add("P_COMPKY", "A001");
            requestDic.Add("P_WAREKY", "10");
            requestDic.Add("P_RQSHPD", "2019-04-01");
            requestDic.Add("P_DLWRKY", "A31");
            requestDic.Add("P_RUTEKY", "A31");
            requestDic.Add("P_DLVYCD", "A0062750");

            //requestDic.Add("P_SHTT01", this.IsTranToggle ? "V" : " "); //완료 'V' or 미완료 ' '


            responseResult = await BaseHttpService.Instance.SendRequestAsync(HttpCommand.GET, requestDic);

 

            if (string.IsNullOrEmpty(responseResult) || responseResult.StartsWith("ERROR"))
            {
                if (responseResult.StartsWith("ERROR"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", responseResult, "OK");
                }

                IsBusy = false;
                IsEnabled = true;

                //this.SearchResult = new ObservableCollection<LIBR020Model>(); //생성해야 null 에러 안남

                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(responseResult))
                {

                    _listSearchResult = JsonConvert.DeserializeObject<List<LOBSM020Model>>(responseResult);

                    this.RowTotal = _listSearchResult.Count;


                    foreach (var item in _listSearchResult)
                    {
                        Debug.WriteLine(string.Format("{0},{1}", item.Slipno, item.Prodnm));

                    }



                    this.RowTotal = _listSearchResult.Count;
                    this.SearchResult.AddRange(_listSearchResult, System.Collections.Specialized.NotifyCollectionChangedAction.Reset);
                    //_listSearchResult = null;
                    _listSearchResult.Clear();
                }
            }

            IsBusy = false;
            IsEnabled = true;
        }

        public GridControl Grid
        {
            get { return _gridControl; }
            set { _gridControl = value; }
        }

        //public ObservableCollection<LIBR020Model> SearchResult { get => _searchResult; set => SetProperty(ref this._searchResult, value); }
     //   public ObservableRangeCollection<LOBSM020Model> SearchResult { get => _searchResult; set => SetProperty(ref this._searchResult, value); }

        public ObservableRangeCollection<LOBSM020Model> SearchResult
        {
            get => _searchResult;
            set => SetProperty(ref this._searchResult, value);
        }

        public string Recvky { get => _recvky; set => SetProperty(ref _recvky, value); }
        public string Pltid { get => _pltid; set => SetProperty(ref _pltid, value); }
        public string TranName { get => _tranName; set => SetProperty(ref _tranName, value); }
        public bool IsTranToggle { get => _isTranToggle; set => SetProperty(ref _isTranToggle, value); }
        public int RowTotal { get => _rowTotal; set => SetProperty(ref _rowTotal, value); }

    }
}
