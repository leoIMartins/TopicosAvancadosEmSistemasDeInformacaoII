using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Evento
{
    public partial class Viagem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void BtnSalvar_Click(object sender, EventArgs e)
        {
            Model.Ambiente ambiente = getData();
            var db = new AmbienteDB();

            if ((IdH.Value == "0") || (IdH.Value == ""))
            {
                if (db.Insert(ambiente))
                {
                    LblMsg.Text = "Registro inserido!";
                    LoadGrid();
                }
                else
                    LblMsg.Text = "Erro ao inserir registro";
            }
            else
            {
                if (db.Update(ambiente, int.Parse(IdH.Value)))
                {
                    LblMsg.Text = "Registro atualizado!";
                }
                else
                    LblMsg.Text = "Erro ao atualizar registro";
            }

            LoadGrid();
        }

        private Model.Ambiente getData()
        {
            return new Model.Ambiente()
            {
                Descricao = TxtDescricao.Text,
            };
        }

        private void LoadGrid()
        {
            GVAmbiente.DataSource = new AmbienteDB().GetAll();
            GVAmbiente.DataBind();
        }

        protected void GVAmbiente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVAmbiente.Rows[index];

            int id = Convert.ToInt32(row.Cells[0].Text);

            var db = new AmbienteDB();

            if (e.CommandName == "Excluir")
            {
                db.Delete(id);
                LoadGrid();

            }
            else if (e.CommandName == "Editar")
            {
                Model.Ambiente ambiente = db.SelectById(id);

                TxtDescricao.Text = ambiente.Descricao;
                IdH.Value = ambiente.Id.ToString();
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            TxtDescricao.Text = "";
            IdH.Value = "0";
            TxtDescricao.Focus();
        }
    }
}