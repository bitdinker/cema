using Cema.Data;
using Cema.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Cema.Components.Pages
{
    public partial class CarList : ComponentBase
    {
        [Inject]
        public DatabaseHelper DatabaseHelper { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<Car> Cars { get; set; } = new List<Car>();

        protected override async Task OnInitializedAsync()
        {
            // The GetCars method now returns the total expenses.
            Cars = DatabaseHelper.GetCars();
        }

        public void AddNewCar()
        {
            NavigationManager.NavigateTo("/addcar"); // Placeholder route
        }

        public void EditCar(int carId)
        {
            NavigationManager.NavigateTo($"/editcar/{carId}"); // Placeholder route
        }

        public void DeleteCar(int carId)
        {
            // Implement confirmation dialog later
            DatabaseHelper.DeleteCar(carId);
            LoadCars(); // Reload the list after deletion
        }

        public void ViewExpenses(int carId)
        {
            NavigationManager.NavigateTo($"/viewexpenses/{carId}"); // Placeholder route
        }
    }
}