using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using cema.Data;
using cema.Models;

namespace cema.Components.Pages
{
    public partial class ExpenseGroupList : ComponentBase
    {
        [Inject]
        public DatabaseHelper DatabaseHelper { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<ExpenseGroup> ExpenseGroups { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ExpenseGroups = DatabaseHelper.GetExpenseGroups();
        }

        public void AddNewGroup()
        {
            NavigationManager.NavigateTo("/expensegroups/add");
        }

        public void EditGroup(int id)
        {
            // Placeholder for editing a group
            NavigationManager.NavigateTo($"/expensegroups/edit/{id}");
        }

        public void DeleteGroup(int id)
        {
            // Placeholder for deleting a group
            System.Console.WriteLine($"Delete group with id: {id}");
            // Implement deletion logic and refresh the list
        }
    }
}