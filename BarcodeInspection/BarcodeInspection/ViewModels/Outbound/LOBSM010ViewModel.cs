using BarcodeInspection.Controls;
using BarcodeInspection.Helpers;
using BarcodeInspection.Models.Outbound;
using BarcodeInspection.Services;
using BarcodeInspection.Views.Outbound;
using DevExpress.Mobile.DataGrid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Forms;

namespace BarcodeInspection.ViewModels.Outbound
{
    public class LOBSM010ViewModel : ViewModelBase
    {

        private GridControl _gridControl;
        private string _compky = string.Empty;
        private DateTime _deptid1 = Convert.ToDateTime("1900-01-01"); //입고예정일
        private string _dlvynm = string.Empty;
        private bool _isTranToggle = false;
        private int _rowTotal = 0;
        private string _lb1count = string.Empty;
        private int _selectedCount = 0;
        private int _rowHandle = -1;

        private ObservableRangeCollection<LOBSM010Model> _searchResult = new ObservableRangeCollection<LOBSM010Model>();

        private List<LOBSM010Model> _listSearchResult = new List<LOBSM010Model>();

        //전체 리스트
        private List<string> AllScanBarcode = new List<string>();
        //스캔 완료 바코드
        private List<string> ScanCompletedBarcode = new List<string>();
        //스캔 완료해서 저장처리한 바코드
        private List<string> SaveCompletedBarcode = new List<string>();

        public ICommand SwitchToggledCommand { get; }
        public ICommand GridRowTapCommand { get; }
        public ICommand BarcodeScanCommand { get; }
        public ICommand EntrySebelnCompletedCommand { get; }
        public ICommand DateSelectedCommand { get; }
        public ICommand BindingContextChangedCommand { get; }
        public ICommand GridSelectionChangedCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand PullToRefreshCommand { get; }
        public ICommand SearchDetailCommand { get; }
        public ICommand SearchCommand { get; }

        public LOBSM010ViewModel()
        {
  //          Deptid1 = DateTime.Now;
   //         TranName = "미완료";

            SwitchToggledCommand = new Command<ToggledEventArgs>(async (e) => SwitchToggled(e));
            GridRowTapCommand = new Command<DevExpress.Mobile.DataGrid.RowTapEventArgs>(async (e) => await GridRowTap(e));
            BarcodeScanCommand = new Command(async () => await BarcodeScan());
            EntrySebelnCompletedCommand = new Command(async () => await EntrySebelnCompleted());
            DateSelectedCommand = new Command(async () => await DateSelected());
            BindingContextChangedCommand = new Command(async () => await BindingContextChanged());
            ClearCommand = new Command(() => Clear());
            PullToRefreshCommand = new Command(async () => await PullToRefresh());
            SearchDetailCommand = new Command(async () => await SearchDetail());
            SearchCommand = new Command(async () => await GetLOBSM010());
        }

        private async Task BindingContextChanged()
        {
            await GetLOBSM010();
        }

        private async Task PullToRefresh()
        {
            await GetLOBSM010();
        }

        public GridControl Grid
        {
            get { return _gridControl; }
            set { _gridControl = value; }
        }

        public void Clear()
        {
            this.SearchResult.Clear();
     //       this.Sebeln = string.Empty;
        }

        public async Task DateSelected()
        {
            await GetLOBSM010();
        }

        //상세내역 페이지로 이동
        private async Task EntrySebelnCompleted(Entry e)
        {
            Console.WriteLine(e.Text);

            if (!string.IsNullOrEmpty(e.Text.Trim()))
            {
  //              this.Sebeln = e.Text.Trim();

                await SearchDetail();
            }
        }

        private async Task EntrySebelnCompleted()
        {
            //if (!string.IsNullOrEmpty(Dlvynm.Trim()))
            //{
            //    await SearchDetail();
            //}
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

            this.Lb1Count = string.Empty;
            this.Grid.SelectedRowHandle = -1;

            //scanbarcode scanbarcode = new scanbarcode(false, false, allscanbarcode, scancompletedbarcode, savecompletedbarcode);
            //scanbarcode.onscancompleted += (list<string> result) =>
            //{
            //    if (result.count > 0)
            //    {
            //        //lobl020_scancompleted(result);

            //        //이렇게 하지 않으면 에러 발생함.
            //        //아이폰에서만 에러
            //        device.begininvokeonmainthread(async () =>
            //        {
            //            await libr010_scancompleted(result);
            //        });
            //    }
            //};

            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //    await Application.Current.MainPage.Navigation.PushAsync(scanBarcode);
            //}
            //else if (Device.RuntimePlatform == Device.Android)
            //{
            //    await Application.Current.MainPage.Navigation.PushModalAsync(scanBarcode);
            //}

            IsBusy = false;
            IsEnabled = true;
        }

        //상세내역 페이지로 이동
        public async Task LIBR010_ScanCompleted(List<string> scanResult)
        {
            //IsBusy = true;
            //IsEnabled = false;
            //foreach (var scanItem in scanResult) //카메라 화면에서 받아 온 것 
            //{
            //    foreach (var item in SearchResult) //화면에 있는 데이터
            //    {
            //        if (item.Sebeln == scanItem)
            //        {
            //            this.Sebeln = scanItem;

            //            //해당 라인으로 포커스 이동
            //            int rowNum = this.Grid.FindRowByValue("Sebeln", scanItem);
            //            this.Grid.SelectedRowHandle = rowNum; // 선택
            //            this.Grid.ScrollToRow(rowNum); //해당 Row로 이동

            //            this.Sebeln = this._gridControl.GetCellValue(rowNum, "Sebeln").ToString();
            //            //IRowData rowData = this._gridControl.GetRow(0);
            //            this._recvky = this._gridControl.GetCellValue(rowNum, "Recvky").ToString();

            //            //ToDo
            //            await SearchDetail();

            //            break;
            //        }
            //    }
            //}

            //LabelCount = SearchResult.Count;
            //IsBusy = false;
            //IsEnabled = true;
        }

        private void BarcodeCount()
        {
            this.RowTotal = SearchResult.Count;
            //this.LabelCount = "[" + SaveScanResult.Count.ToString() +"/"+ ScanCompletedBarcode.Count.ToString() + "/" + this.SaveCompletedBarcode.Count.ToString() + "/" + AllScanBarcode.Count.ToString() + "]";
        }

        private async Task GridRowTap(RowTapEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                //      this.Dlvynm = this._gridControl.GetCellValue(e.RowHandle, "Dlvynm").ToString();
                //         IRowData rowData = this._gridControl.GetRow(0);
                //       this._lb1count = this._gridControl.GetCellValue(e.RowHandle, "Lb1Count").ToString();


  //              if (this._rowHandle != -1 && this._rowHandle == e.RowHandle)
   //             {
  //                  this._selectedCount += 1;
   //                 if (this._selectedCount == 2)
   //                 {
                        //Todo
                        //Grid Row Double Click
                        this._selectedCount = 0;

                        await SearchDetail();
     //               }
   //             }
   //             else
     //           {
     //               this._rowHandle = e.RowHandle;
     //               this._selectedCount = 1;
      //          }
            }
            //this._gridControl.SelectedRowHandle; //선택된 row
        }

        private async Task SearchDetail()
        {
            IsEnabled = false;

            //if (string.IsNullOrEmpty(this.Sebeln))
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

            //          if (!string.IsNullOrEmpty(this.Dlvynm))
            //          {

            await Application.Current.MainPage.Navigation.PushAsync(new LOBSM020View(this.Compky, this._compky, this.IsTranToggle));
   //         }

            IsEnabled = true;
        }

        private void SwitchToggled(ToggledEventArgs e)
        {
     //       this.TranName = e.Value ? "완료" : "미완료";
            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetLOBSM010();
            });
        }

        public async Task GetLOBSM010()
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

            //if (string.IsNullOrEmpty(this.Sebeln))
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
            //requestDic.Add("CMD", "{call ufn_get_lobsm010(?, ?, ?, ?)}"); //프로시저
            //requestDic.Add("P_COMPKY", Settings.Compky);
            //requestDic.Add("P_WAREKY", Settings.Wareky);
            //requestDic.Add("P_RQSHPD", this.Deptid1.ToString("yyyyMMdd"));
            //requestDic.Add("P_DLWRKY", Settings.Dlwrky);
            //requestDic.Add("P_RUTEKY", Settings.Ruteky);

            requestDic.Add("UFN", "{?=call ufn_get_lobsm010(?, ?, ?, ?,?)}"); //프로시저
            requestDic.Add("P_COMPKY", "A001");
            requestDic.Add("P_WAREKY", "10");
            requestDic.Add("P_RQSHPD", "2019-04-01");
            requestDic.Add("P_DLWRKY", "A21");
            requestDic.Add("P_RUTEKY", "A21");


            //       requestDic.Add("P_SHTT01", this.IsTranToggle ? "V" : " "); //완료 'V' or 미완료 ' '

            responseResult = await BaseHttpService.Instance.SendRequestAsync(HttpCommand.GET, requestDic);

            if (string.IsNullOrEmpty(responseResult) || responseResult.StartsWith("ERROR"))
            {
                if (responseResult.StartsWith("ERROR"))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", responseResult, "OK");
                }

                IsBusy = false;
                IsEnabled = true;



                //★생성해야 null 에러 안남
                //this.SearchResult = new ObservableCollection<LIBR010Model>(); 

                return;
            }
            else
            {
   
                //               XDocument doc = XDocument.Parse(responseResult);
                //              XElement ele = doc.Element(XName.Get("ROWSET"));




     

                _listSearchResult = JsonConvert.DeserializeObject<List<LOBSM010Model>>(responseResult);
                //_listSearchResult =
                //    ele.Elements(XName.Get("ROW")).Select((XElement element) =>
                //    {
                //       return new LOBSM010Model(element);
                //    }).ToList();


                this.RowTotal = _listSearchResult.Count;
         

                foreach (var item in _listSearchResult)
                {
                    Debug.WriteLine(string.Format("{0},{1}", item.Compky, item.Wareky));

                }

     //           this.SearchResult = new ObservableCollection<LOBSM010Model>(_listSearchResult);
                this.SearchResult.AddRange(_listSearchResult, System.Collections.Specialized.NotifyCollectionChangedAction.Reset);
                //_listSearchResult = null;
                _listSearchResult.Clear();

     

          //      this.Grid.SelectedDataObject = null; //선택한걸 해제 시킨다.

          
            }

            IsBusy = false;
            IsEnabled = true;
        }

        public ObservableRangeCollection<LOBSM010Model> SearchResult
        {
            get => _searchResult;
            set => SetProperty(ref this._searchResult, value);
        }
        public string Compky { get => _compky; set => SetProperty(ref _compky, value); }
        public string Dlvynm { get => _dlvynm; set => SetProperty(ref _compky, value); }
        public string Lb1Count { get => _lb1count; set => SetProperty(ref _lb1count, value); }
        public bool IsTranToggle { get => _isTranToggle; set => SetProperty(ref _isTranToggle, value); }
        public int RowTotal { get => _rowTotal; set => SetProperty(ref _rowTotal, value); }


    }
}
