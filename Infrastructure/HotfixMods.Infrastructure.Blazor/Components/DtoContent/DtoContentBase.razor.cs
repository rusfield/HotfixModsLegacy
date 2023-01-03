using HotfixMods.Infrastructure.Blazor.Components.Dialogs;
using HotfixMods.Infrastructure.Blazor.PageData;
using HotfixMods.Infrastructure.DtoModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class DtoContentBase<TDto, TValue> : ComponentBase
        where TDto : IDto
        where TValue : class
    {
        [CascadingParameter(Name = "PageTab")]
        public PageTab PageTab { get; set; }
        [Parameter]
        public string? GroupName { get; set; }
        [Inject]
        public IDialogService DialogService { get; set; }
        public TValue Value { get; set; }
        public TValue? ValueCompare { get; set; }

        protected override void OnParametersSet()
        {
            var dtoType = PageTab.Dto.GetType();
            var dtoProperty = dtoType.GetProperty(typeof(TValue).Name);
            Value = (TValue)dtoProperty.GetValue(PageTab.Dto);
            base.OnParametersSet();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (PageTab.DtoCompare != null)
            {
                var dtoCompareType = PageTab.DtoCompare.GetType();
                var dtoCompareProperty = dtoCompareType.GetProperty(typeof(TValue).Name);
                ValueCompare = (TValue)dtoCompareProperty.GetValue(PageTab.DtoCompare);
            }
            else
            {
                ValueCompare = null;
            }
            base.OnAfterRender(firstRender);
        }

        protected EventCallback OpenInfoDialog(string infoText)
        {
            var parameters = new DialogParameters();
            parameters.Add(nameof(InfoButton_Dialog.Text), infoText);
            return new EventCallbackFactory().Create(this, () => DialogService.Show<InfoButton_Dialog>(null, parameters));
        }

        protected EventCallback OpenLookupDialog(string db2Name, int id)
        {
            return new EventCallbackFactory().Create(this, async () => await LookupDialogEventCallback(db2Name, id));
        }

        async Task LookupDialogEventCallback(string db2Name, int id)
        {
            /*
           var parameters = new DialogParameters();
           parameters.Add(nameof(GenericHotfixSearch_Dialog.Db2Name), db2Name);
           parameters.Add(nameof(GenericHotfixSearch_Dialog.Id), id);
           var data = await DialogService.Show<GenericHotfixSearch_Dialog>(null, parameters).Result;

           if (!data.Cancelled)
           {

               GlobalHandler.LaunchTab?.Invoke(new PageTab<IDto>(db2Name, typeof(GenericHotfixes))
               {
                   //Dto = (DbRow)data.Data
               });
           }
               */
        }
    }
}
