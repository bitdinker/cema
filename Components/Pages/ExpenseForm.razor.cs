using Microsoft.AspNetCore.Components;

namespace Cema.Components.Pages
{
    public partial class ExpenseForm : ComponentBase
    {

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int? ExpenseId { get; set; }

        [Parameter]
        public int CarId { get; set; }

        public Expense Expense { get; set; } = new Expense();
        public List<ExpenseGroup> ExpenseGroups { get; set; } = new List<ExpenseGroup>();

        protected override async Task OnInitializedAsync()
        {
            ExpenseGroups = DatabaseHelper.GetExpenseGroups(); // Assuming this method exists

            if (ExpenseId.HasValue)
            {
                // Assuming GetExpenseById method exists in DatabaseHelper
                Expense = DatabaseHelper.GetExpenseById(ExpenseId.Value);
            }
            else
            {
                Expense.CarId = CarId;
                Expense.Date = DateTime.Today; // Default date
            }
        }

        private void HandleSave()
        {
            if (ExpenseId.HasValue)
            {
                DatabaseHelper.UpdateExpense(Expense); // Assuming UpdateExpense method exists
            }
            else
            {
                DatabaseHelper.InsertExpense(Expense); // Assuming InsertExpense method exists
            }

            NavigationManager.NavigateTo($"/expenses/{CarId}");
        }

        private void HandleCancel()
        {
            NavigationManager.NavigateTo($"/expenses/{CarId}");
        }
    }
}