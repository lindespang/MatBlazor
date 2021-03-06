﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MatBlazor
{
    /// <summary>
    /// Nav Item is a menu item in the Nav Menu. Inherits from Mat List Item.
    /// </summary>
    public class BaseMatNavItem : BaseMatListItem
    {
        [Inject]
        public NavigationManager UriHelper { get; set; }

        [CascadingParameter]
        public BaseMatNavMenu MatNavMenu { get; set; }

        [CascadingParameter]
        public BaseMatNavSubMenu MatNavSubMenu { get; set; }

        /// <summary>
        ///  Command executed when the user clicks on an element.
        /// </summary>
        [Parameter]
        public ICommand Command { get; set; }

        /// <summary>
        ///  Command parameter.
        /// </summary>
        [Parameter]
        public object CommandParameter { get; set; }

        [Parameter]
        public bool Selected { get; set; }

        /// <summary>
        /// Specifies weather you the Nav Item can be selected / active.
        /// </summary>
        [Parameter]
        public bool AllowSelection { get; set; } = true;

        [Parameter]
        public EventCallback<bool> SelectedChanged { get; set; }

        public async Task ToggleSelectedAsync()
        {
            this.Selected = !this.Selected;

            await SelectedChanged.InvokeAsync(this.Selected);

            if (MatNavMenu != null)
            {
                await this.MatNavMenu.ToggleSelectedAsync(this, MatNavSubMenu);
            }

            this.StateHasChanged();
        }

        public BaseMatNavItem()
        {
            ClassMapper
                .Add("mdc-nav-item")
                .If("mdc-list-item--selected", () => (Selected && AllowSelection));
        }

        /// <summary>
        ///  OnClickHandler parameter.
        /// </summary>
        protected async void OnClickHandler(MouseEventArgs e)
        {
            if (Disabled) return;

            if (AllowSelection)
            {
                await this.ToggleSelectedAsync();
            }

            if (Href != null)
            {
                UriHelper.NavigateTo(Href);
            }
            else
            {
                await OnClick.InvokeAsync(e);
                if (Command?.CanExecute(CommandParameter) ?? false)
                {
                    Command.Execute(CommandParameter);
                }
            }
        }
    }
}
