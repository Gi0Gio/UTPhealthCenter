using System;
using Microsoft.Maui.Controls;

namespace HealthCare.Views
{
    public partial class MedicinesPage : ContentPage
    {
        public MedicinesPage()
        {
            InitializeComponent();
        }

        private void ShowMedicineList(object sender, EventArgs e)
        {
            MedicineListSection.IsVisible = true;
            AssignMedicineSection.IsVisible = false;
            MedicineFormSection.IsVisible = false;
        }

        private void ShowAssignMedicine(object sender, EventArgs e)
        {
            MedicineListSection.IsVisible = false;
            AssignMedicineSection.IsVisible = true;
            MedicineFormSection.IsVisible = false;
        }

        private void ShowMedicineForm(object sender, EventArgs e)
        {
            MedicineListSection.IsVisible = false;
            AssignMedicineSection.IsVisible = false;
            MedicineFormSection.IsVisible = true;
        }

        private void OnSaveMedicine(object sender, EventArgs e)
        {
            // Lógica para guardar la nueva medicina
        }

        private void OnAssignMedicine(object sender, EventArgs e)
        {
            // Lógica para asignar la medicina al paciente
        }
    }
}
