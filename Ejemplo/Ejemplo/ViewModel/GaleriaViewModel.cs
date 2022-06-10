using Ejemplo.Models;
using Ejemplo.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Proyecto.ViewModel
{
    public class GaleriaViewModel
        {
        private ObservableCollection<GaleriaArte> listagaleria;
        public ObservableCollection<GaleriaArte> ListaGaleria { get; set; } = new ObservableCollection<GaleriaArte>();

        private GaleriaArte galeria;

        public GaleriaArte Galeria
        {
            get { return galeria; }
            set { galeria = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Galeria")); }
        }
        public string Error { get; set; } = "";

        AgregarView agregardialog;
        DetallesView detallesdialog;
        EditarView editardialog;

        public int posicionOriginal;
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand DetallesCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand EditarCommand { get; set; }


        public GaleriaViewModel()
        {
            Open();
            CambiarVistaCommand = new Command<string>(CambiarVista);
            AgregarCommand = new Command(Agregar);
            DetallesCommand = new Command<GaleriaArte>(Detalles);
            EliminarCommand = new Command<GaleriaArte>(Message);
            VerEditarCommand = new Command<GaleriaArte>(VerEditar);
            EditarCommand = new Command(Editar);
        }
        private void CambiarVista(string vista)
        {
            if (vista == "Agregar")
            {
                Galeria = new GaleriaArte();
                agregardialog = new AgregarView() { BindingContext = this };
                Application.Current.MainPage.Navigation.PushAsync(agregardialog);
            }
            else if (vista == "Inicio")
            {
                Application.Current.MainPage.Navigation.PopToRootAsync();
            }
        }
        private void Agregar()
        {
            if (string.IsNullOrWhiteSpace(Galeria.Nombre))
            {
                Error = "Escriba el titulo de la obra";
                return;
            }
            if (string.IsNullOrWhiteSpace(Galeria.Autor))
            {
                Error = "Indique el nombre del autor";
                return;
            }
            if (Galeria.Precio < 0)
            {
                Error = "Indique un precio valido";
                return;
            }
            if (string.IsNullOrWhiteSpace(Galeria.Imagen))
            {
                Error = "Introduzca la url de la imagen";
                return;
            }
            ListaGaleria.Add(Galeria);
            Save();
            CambiarVista("Inicio");
            Change();
        }

        public void VerEditar(GaleriaArte ga)
        {

            if (ga != null)
            {
                posicionOriginal = ListaGaleria.IndexOf(ga);

                this.Galeria = new GaleriaArte
                {
                    Nombre = ga.Nombre,
                    Autor = ga.Autor,
                    Imagen = ga.Imagen,
                    Precio = ga.Precio,
                };

                editardialog = new EditarView()
                {
                    BindingContext = this
                };

                Application.Current.MainPage.Navigation.PushAsync(editardialog);
            }
        }
        private void Editar()
        {
            Error = "";
            if (Galeria != null)
            {
                if (string.IsNullOrWhiteSpace(Galeria.Nombre))
                {
                    Error = "Escriba el titulo de la obra";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Galeria.Autor))
                {
                    Error = "Indique el nombre del autor";
                    return;
                }
                if (Galeria.Precio < 0)
                {
                    Error = "Indique un precio valido";
                    return;
                }
                if (string.IsNullOrWhiteSpace(Galeria.Imagen))
                {
                    Error = "Introduzca la url de la imagen";
                    return;
                }
                if (Error == "")
                {
                    {
                        ListaGaleria[posicionOriginal] = Galeria; 
                        Save();
                        Application.Current.MainPage.Navigation.PopToRootAsync();
                        Change();
                    }
                }
            }
        }

        private void Guardar()
        {

            ListaGaleria[posicionOriginal] = Galeria;
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }
        private void Detalles(GaleriaArte ga)
        {
            detallesdialog = new DetallesView()
            {
                BindingContext = this
            };
            this.Galeria = ga;
            Change();
            Application.Current.MainPage.Navigation.PushAsync(detallesdialog);
        }

        public void Eliminar(GaleriaArte ga)
        {
            if (ga != null)
            {
                ListaGaleria.Remove(ga);
                Change();
                Save();
                Application.Current.MainPage.Navigation.PopToRootAsync();
            }
            Change();
        }
        private async void Message(GaleriaArte ga)
        {
            bool respuesta = await Application.Current.MainPage.DisplayAlert("Delete", "Desea eliminar el siguiente elemento?", "Sí", "No");
            if (respuesta == true)
            {
                Galeria = ga;
                Eliminar(ga);
            }
            Change();
        }

        private void Save()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "galeria.json";
            File.WriteAllText(file, JsonConvert.SerializeObject(ListaGaleria));
        }
        void Open()
        {
            var file = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "galeria.json";
            if (File.Exists(file))
            {
                ListaGaleria = JsonConvert.DeserializeObject<ObservableCollection<GaleriaArte>>(File.ReadAllText(file));
            }
        }
        private void Change(string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;

    }
}
