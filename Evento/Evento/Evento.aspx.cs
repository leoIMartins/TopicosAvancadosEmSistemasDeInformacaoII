using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Evento
{
    public partial class Evento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarDadosPagina();
            }
        }

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            string descricao = txtDescricao.Text;
            string data = txtData.Text;
            int qtdPessoas = Convert.ToInt32(txtQtdPessoas.Text);
            int qtdMaxPermitida = Convert.ToInt32(txtQtdMaxPermitida.Text);
            decimal valor = Convert.ToDecimal(txtValorPorPessoa.Text);
            TB_Evento te = new TB_Evento()
            {
                NOME_ORGAO = descricao,
                SIGLA_ORGAO = sigla,
                APROVADA = aprovadas,
                DISTRIBUIDA = qtdMaxPermitida,
                OCUPADA = valor,
                VAGAS = vagasDesocupadas
            };
            dbcargosvagosEntities contextVagas = new dbcargosvagosEntities();

            string valor = Request.QueryString["orgao"];

            if (String.IsNullOrEmpty(valor))
            {
                contextVagas.TB_Evento.Add(te);
                lblmsg.Text = "Registro Inserido!";
                Clear();
            }
            else
            {
                int id = Convert.ToInt32(valor);
                TB_Evento vagas = contextVagas.TB_Evento.First(v => v.ORGAO == id);
                vagas.NOME_MES = te.NOME_MES;
                vagas.ORGAO = te.ORGAO;
                vagas.NOME_ORGAO = te.NOME_ORGAO;
                vagas.SIGLA_ORGAO = te.SIGLA_ORGAO;
                vagas.APROVADA = te.APROVADA;
                vagas.DISTRIBUIDA = te.DISTRIBUIDA;
                vagas.OCUPADA = te.OCUPADA;
                vagas.VAGAS = te.VAGAS;
                lblmsg.Text = "Registro Alterado";
            }
            contextVagas.SaveChanges();
        }

        private void Clear()
        {
            txtMes.Text = "";
            txtOrgao.Text = "";
            txtNomeOrgao.Text = "";
            txtSigla.Text = "";
            txtAprovadas.Text = "";
            txtDistribuidas.Text = "";
            txtOcupadas.Text = "";
            txtMes.Focus();
        }

        private void CarregarDadosPagina()
        {
            string valor = Request.QueryString["orgao"];
            int orgao = 0;
            TB_Evento vagas = new TB_Evento();


            if (!String.IsNullOrEmpty(valor))
            {
                dbcargosvagosEntities contextCamisetas = new dbcargosvagosEntities();
                orgao = Convert.ToInt32(valor);
                vagas = contextCamisetas.TB_Evento.First(v => v.ORGAO == orgao);

                txtMes.Text = vagas.NOME_MES;
                txtOrgao.Text = vagas.ORGAO.ToString();
                txtOrgao.ReadOnly = true;
                txtNomeOrgao.Text = vagas.NOME_ORGAO;
                txtSigla.Text = vagas.SIGLA_ORGAO;
                txtAprovadas.Text = vagas.APROVADA.ToString();
                txtDistribuidas.Text = vagas.DISTRIBUIDA.ToString();
                txtOcupadas.Text = vagas.OCUPADA.ToString();
            }
        }
    }
}