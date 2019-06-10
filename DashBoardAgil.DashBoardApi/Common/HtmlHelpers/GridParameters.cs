using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DashBoardAgil.DashBoardApi.Common.HtmlHelpers
{
    public class GridParameters
    {
        private bool? _sendFormData;

        public bool SendFormData
        {
            get { return _sendFormData ?? true; }
            set { _sendFormData = value; }
        }

        public string IdTable { get; set; }
        public Order OrderDefault { get; set; }
        public string Controller { get; set; }
        public IList<Columns> Columns { get; set; }
        public IList<Filter> Filters { get; set; }
        private bool? _autoLoad;
        public bool AutoLoad
        {
            get { return _autoLoad ?? true; }
            set { _autoLoad = value; }
        }
        private bool? _ordering;
        public bool Ordering
        {
            get { return _ordering ?? true; }
            set { _ordering = value; }
        }

        private bool? _paging;
        public bool Paging
        {
            get { return _paging ?? true; }
            set { _paging = value; }
        }

        private bool? _isVisiblePrintButton;
        public bool IsVisiblePrintButton
        {
            get { return _isVisiblePrintButton ?? true; }
            set { _isVisiblePrintButton = value; }
        }

        private bool? _isVisibleExcelButton;
        public bool IsVisibleExcelButton
        {
            get { return _isVisibleExcelButton ?? false; }
            set { _isVisibleExcelButton = value; }
        }

        private int? _pageLength;
        public int PageLength
        {
            get { return _pageLength ?? 10; }
            set { _pageLength = value; }
        }

        private string _printPageTitleSize;
        public string PrintPageTitleSize
        {
            get { return _printPageTitleSize ?? "36px"; }
            set { _printPageTitleSize = value; }
        }

        public string PrintPageCustomTitle { get; set; }

        private int? _cachePageLength;
        public int CachePageLength
        {
            get { return _cachePageLength ?? 5; }
            set { _cachePageLength = value; }
        }

        private string _emptyTextTable;
        public string EmptyTextTable
        {
            get { return _emptyTextTable ?? "Nenhum registro encontrado."; }
            set { _emptyTextTable = value; }
        }

        private string _wrapperIdBtnImprimir;
        public string WrapperIdBtnImprimir
        {
            get { return _wrapperIdBtnImprimir ?? "controlPanel"; }
            set { _wrapperIdBtnImprimir = value; }
        }

        private string _wrapperIdFiltroImprimir;
        public string WrapperIdFiltroImprimir
        {
            get { return _wrapperIdFiltroImprimir ?? "filtro"; }
            set { _wrapperIdFiltroImprimir = value; }
        }

        private string _wrapperIdRodapeImprimir;
        public string WrapperIdRodapeImprimir
        {
            get { return _wrapperIdRodapeImprimir ?? "rodapeImprimir"; }
            set { _wrapperIdRodapeImprimir = value; }
        }
    }

    public class Filter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Columns
    {
        public string DataField { get; set; }
        public string DataFieldAlias { get; set; }
        public bool Searchable { get; set; }
        public bool Sortable { get; set; }
        public bool DefaultContent { get; set; }
        public string ClassName { get; set; }
        public TypeColumn TypeColumn { get; set; }
        public List<Link> Links { get; set; }

    }
    public class Link
    {
        public string DataFieldText { get; set; }
        public bool IsIcon { get; set; }
        public string Class { get; set; }
        public bool IsTargetBlank { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValueDataField { get; set; }
        public IDictionary<string, string> ParametersNamesAndValuesDataField { get; set; }
        public string UrlViaDataField { get; set; }
        public string AbsoluteUrl { get; set; }
        public bool IsBlank { get; set; }
        public string VisibleIf { get; set; }
        public string ModalTitle { get; set; }
        public string TitleAlt { get; set; }
        public string JavaScriptFunction { get; set; }
        public string JavaScripParamName { get; set; }
        public TypeLink TypeLink { get; set; }
        public bool ModalResponsive { get; set; }
        public string ModalWidth { get; set; }
        public string Modalheight { get; set; }

        private string _callBackFunction;
        public string CallBackFunction
        {
            get { return _callBackFunction ?? "CallBackFunction"; }
            set { _callBackFunction = value; }
        }
        public HttpMethod HttpMethod { get; set; }
    }

    public class Order
    {
        public int IndexColumn { get; set; }
        public DirectionOrder Direction { get; set; }
    }
    public enum HttpMethod
    {
        /// <summary>
        /// GET
        /// </summary>
        [Display(Name = "GET")]
        Get = 0,

        /// <summary>
        /// POST
        /// </summary>
        [Display(Name = "POST")]
        Post = 1,
    }

    public enum DirectionOrder
    {
        /// <summary>
        /// Ascendente
        /// </summary>
        [Display(Name = "asc")]
        Asc = 0,

        /// <summary>
        /// Descendente
        /// </summary>
        [Display(Name = "desc")]
        Desc = 1,
    }

    public enum TypeColumn
    {
        Normal = 0,
        CheckBox = 1,
        Links = 2
    }
    public enum TypeLink
    {
        Padrao = 0,
        Modal = 1,
        JavaScript = 2,
        UrlViaDataField = 3,
        AbsoluteUrl = 4,
        Remove = 5
    }
}

