using Microsoft.AspNetCore.Components;
using cema.Data;
using cema.Models;
using System.Threading.Tasks;

namespace cema.Components.Pages
{
    public partial class CarForm : ComponentBase
    {
        [Inject]
        public DatabaseHelper DatabaseHelper { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int Id { get; set; }

        public Car Car { get; set; } = new Car();

        protected override async Task OnInitializedAsync()
        {
            if (Id != 0)
            {
                // In a real application, you'd fetch a single car by Id
                // This is a simplified placeholder. You'll need a GetCarById method in DatabaseHelper.
                // Car = await DatabaseHelper.GetCarById(Id);
                 var allCars = await DatabaseHelper.GetCars();
                 Car = allCars.FirstOrDefault(c => c.Id == Id) ?? new Car();

            }
        }

        public async Task SaveCar()
        {
            if (Car.Id == 0)
            {
                await DatabaseHelper.InsertCar(Car);
            }
            else
            {
                await DatabaseHelper.UpdateCar(Car);
            }
            NavigationManager.NavigateTo("/cars");
        }

        public void Cancel()
        {
            NavigationManager.NavigateTo("/cars");
        }
    }
}