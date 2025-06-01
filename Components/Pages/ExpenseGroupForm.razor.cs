using Microsoft.AspNetCore.Components;

namespace Cema.Components.Pages
{
    public partial class ExpenseGroupForm : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int? Id { get; set; }

        public ExpenseGroup ExpenseGroup { get; set; } = new ExpenseGroup();

        protected override void OnInitializedAsync()
        {
            if (Id.HasValue && Id.Value > 0)
            {
                // Fetch the expense group for editing
                // You'll need to add a GetExpenseGroupById method to DatabaseHelper
                ExpenseGroup = DatabaseHelper.GetExpenseGroupById(Id.Value);
            }
        }

        public void Save()
        {
            if (ExpenseGroup.Id == 0)
            {
                // Insert new expense group
                DatabaseHelper.InsertExpenseGroup(ExpenseGroup);
            }
            else
            {
                // Update existing expense group
                DatabaseHelper.UpdateExpenseGroup(ExpenseGroup);
            }

            NavigationManager.NavigateTo("/expensegroups");
        }

        public void Cancel()
        {
            NavigationManager.NavigateTo("/expensegroups");
        }
    }
}