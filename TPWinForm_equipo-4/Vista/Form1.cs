﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Vista
{
    public partial class Form1 : Form
    {
        private List<Articulo> listArticulo;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargaDatos();
        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.imagenURL);
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulo.Load("https://sferaone.es/wp-content/uploads/2023/02/placeholder-1.png");
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo aux = new Articulo();
            aux = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            Detalle fr = new Detalle(aux, false);
            fr.ShowDialog();
            CargaDatos();

        }
        //Carga todos los datos de la base de datos en la datagrid
        private void CargaDatos()
        {
            dgvArticulo.DataSource = null;
            ArticuloNegocio art = new ArticuloNegocio();
            listArticulo = art.listar();
            dgvArticulo.DataSource = listArticulo;
            dgvArticulo.Columns[7].Visible = false;   // OCULTA LA COLUMNA.
            cargarImagen(listArticulo[0].imagenURL);
        }

        //Elimina el articulo seleccionado (fila) en la Datagrid
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio datos = new ArticuloNegocio();
            Articulo aux = new Articulo();
            aux = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            DialogResult res;
            res = MessageBox.Show("Desea eliminar este articulo? \n \t" + aux.descripcion.ToString(), "Cuidado", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                datos.Eliminar(aux.ID);
                CargaDatos();

            }
            else
            {
                return;
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;
            string filtro = textBox1.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = listArticulo.FindAll(x => x.nombre.ToUpper().Contains(filtro.ToUpper()) || x.marca.descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listArticulo;
            }

            dgvArticulo.DataSource = null;
            dgvArticulo.DataSource = listaFiltrada;

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Articulo nuevo = new Articulo();
            Detalle agregar = new Detalle(nuevo, true);
            agregar.ShowDialog();
            CargaDatos();
        }

        private void btnCategorias_Click(object sender, EventArgs e)
        {
            frmCategorias cat = new frmCategorias(false);
            cat.ShowDialog();
            CargaDatos();
        }

        private void btnMarcas_Click(object sender, EventArgs e)
        {
            frmCategorias mar = new frmCategorias(true);
            mar.ShowDialog();
            CargaDatos();
        }
    }
}
