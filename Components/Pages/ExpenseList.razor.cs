using Cema.Data;
using Cema.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cema.Components.Pages
{
    public partial class ExpenseList : ComponentBase
    {
        [Inject]
        public DatabaseHelper DatabaseHelper { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int CarId { get; set; }

        public List<Expense> Expenses { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // Fetch expenses for the specified car
            Expenses = await DatabaseHelper.GetExpensesForCarAsync(CarId);
        }

        public void NavigateToAddExpense()
        {
            // Navigate to the Add New Expense form, passing the CarId
            NavigationManager.NavigateTo($"/add-expense/{CarId}");
        }

        // Placeholder methods for Edit and Delete expenses (to be implemented later)
        public void EditExpense(int expenseId)
        {
            // Navigation to edit form will be implemented later
            NavigationManager.NavigateTo($"/edit-expense/{expenseId}");
        }

        public void DeleteExpense(int expenseId)
        {
            // Deletion logic will be implemented later
            // Consider adding a confirmation dialog
        }
    }
}