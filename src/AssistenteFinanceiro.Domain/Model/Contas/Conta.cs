﻿using AssistenteFinanceiro.Domain.Enums;
using AssistenteFinanceiro.Domain.Model.Contas.ValueObjects;
using AssistenteFinanceiro.Domain.Model.ContaValueObjects;
using AssistenteFinanceiro.Infra.SharedKernel.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AssistenteFinanceiro.Domain.Model
{
    public class Conta : BaseEntity
    {
        protected Conta()
        {
        }

        public Conta(NomeConta nome, DescricaoConta descricao, TipoConta tipo, decimal saldoInicial = 0)
        {
            Nome = nome;
            Descricao = descricao;
            Tipo = tipo;
            SaldoInicial = saldoInicial;
            SaldoAtual = saldoInicial;

            Transacoes = new List<Transacao>();
            Orcamentos = new List<Orcamento>();
            Objetivos = new List<Objetivo>();
        }

        public NomeConta Nome { get; }
        public DescricaoConta Descricao { get; }
        public IconeConta Icone { get; }
        public TipoConta Tipo { get; }
        public decimal SaldoInicial { get; }
        public decimal SaldoAtual { get; private set; }

        public ICollection<Transacao> Transacoes { get; }
        public ICollection<Orcamento> Orcamentos { get; }
        public ICollection<Objetivo> Objetivos { get; }

        public IEnumerable<Transacao> Receitas => Transacoes.Where(t => t.IsReceita());
        public IEnumerable<Transacao> Despesas => Transacoes.Where(t => t.IsDespesa());

        public int TransacoesRealizadas() => Transacoes.Count(t => t.Efetivada);
        public int TransacoesPendentes() => Transacoes.Count(t => !t.Efetivada);

        public void AdicionarTransacao(Transacao transacao)
        {
            if (Transacoes.Contains(transacao))
                return;

            if (transacao.IsReceita())
            {
                SaldoAtual += transacao.Valor;
            } 
            else
            {
                SaldoAtual -= transacao.Valor;
            }

            Transacoes.Add(transacao);          
        }

        public void RemoverTransacao(Transacao transacao)
        {
            if (!Transacoes.Contains(transacao))
                return;

            if (transacao.IsDespesa())
            {
                SaldoAtual += transacao.Valor;
            }
            else
            {
                SaldoAtual -= transacao.Valor;
            }

            Transacoes.Remove(transacao);
        }
    }
}
