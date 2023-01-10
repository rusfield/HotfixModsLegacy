using HotfixMods.Infrastructure.Extensions;
using HotfixMods.Infrastructure.Blazor.Components.Dialogs;
using HotfixMods.Infrastructure.Blazor.PageData;
using HotfixMods.Infrastructure.DtoModels;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HotfixMods.Infrastructure.Blazor.Components.DtoContent
{
    public partial class DtoContentBase<TDto, TValue> : ComponentBase
        where TDto : IDto
        where TValue : class, new()
    {
        [CascadingParameter(Name = "PageTab")]
        public PageTab PageTab { get; set; }
        [CascadingParameter(Name = "InstanceData")]
        public InstanceData? InstanceData { get; set; }
        [CascadingParameter(Name = "GroupIndex")]
        public int GroupIndex { get; set; }


        [Inject]
        public IDialogService DialogService { get; set; }

        public TValue? Value { get; set; }
        public TValue? ValueCompare { get; set; }

        protected override void OnParametersSet()
        {
            SetValue();
            if(null == Value)
                SetValueToDefault();
            base.OnParametersSet();
        }

        protected override void OnInitialized()
        {
            SetValue();
            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            SetValue();
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

        void SetValue()
        {
            if(InstanceData == null)
            {
                Value = PageTab.Dto.GetDtoValue<TValue>();
            }
            else
            {
                Value = PageTab.Dto.GetDtoGroupValue<TValue>(InstanceData.GroupType, GroupIndex);
            }
        }

        void SetValueToDefault()
        {
            // The rendering order is a bit bad, and during removal of instances may cause the GroupIndex to be out of range and set Value to null, which in turn crashes the tab pages.
            // This Value should not be visible or used, it will only prevent the exception and the render will clean it up on its own.
            Value = (TValue?)Activator.CreateInstance(typeof(TValue));
        }


        void SetValueCompare()
        {
            // TODO
        }
    }
}
