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
            var dtoType = PageTab.Dto.GetType();
            if(GroupType == null)
            {
                var dtoProperty = dtoType.GetProperty(typeof(TValue).Name);
                Value = (TValue)dtoProperty?.GetValue(PageTab.Dto);
            }
            else
            {
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType);
                PageTab.Dto.AddToGroup(GroupType); // Temp before null chec on Value exists

                var groupProperty = dtoType.GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(List<>) && p.PropertyType.GetGenericArguments()[0] == GroupType).FirstOrDefault();
                var getMethod = groupProperty.PropertyType.GetMethod("get_Item");
                var group = groupProperty.GetValue(PageTab.Dto);
                var count = (int)group.GetType().GetProperty("Count").GetValue(group);
                if(count > 0 && GroupIndex < count)
                {
                    var groupValue = getMethod.Invoke(group, new object[] { GroupIndex });
                    Value = (TValue)groupValue.GetType().GetProperty(typeof(TValue).Name).GetValue(groupValue);
                }
            }
        }

        void ResetValue()
        {
            if(GroupType == null)
            {
                var dtoType = PageTab.Dto.GetType();
                var dtoProperty = dtoType.GetProperty(typeof(TValue).Name);
                dtoProperty.SetValue(PageTab.Dto, Activator.CreateInstance(dtoProperty.PropertyType));
                Value = (TValue)dtoProperty.GetValue(PageTab.Dto);
            }
        }

        void SetValueCompare()
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
        }
    }
}
