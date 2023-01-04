using HotfixMods.Infrastructure.Blazor.BlazorExtensions;
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
        [CascadingParameter(Name = "GroupIndex")]
        public int GroupIndex { get; set; }  

        [Parameter]
        public Type? GroupType { get; set; }

        [Inject]
        public IDialogService DialogService { get; set; }

        public TValue? Value { get; set; }
        public TValue? ValueCompare { get; set; }

        protected override void OnParametersSet()
        {
            ResetValue();
            SetValue();
            base.OnParametersSet();
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
            if(GroupType == null)
            {
                Value = PageTab.Dto.GetDtoValue<TValue>();
            }
            else
            {
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType); // Temp before null check on Value exists

                Value = PageTab.Dto.GetDtoGroupValue<TValue>(GroupType, GroupIndex);
            }
        }

        void ResetValue()
        {
            if(GroupType == null)
            {
                PageTab.Dto.SetDtoValueToDefault<TValue>();
            }
        }

        void SetValueCompare()
        {
            // TODO
        }
    }
}
