﻿using AutoMapper;
using BLL.Interfaces;
using Common;
using Common.Extensions;
using Entities;
using Microsoft.AspNetCore.Mvc;
using WEBPresentationLayer.Models.Cliente;
using WEBPresentationLayer.Models.Funcionario;

namespace MVCPresentationLayer.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly IFuncionarioService _Funcionario;
        private readonly IMapper _mapper;
        private string? funcionario;

        public FuncionarioController(IFuncionarioService svc, IMapper mapper)
        {
            this._Funcionario = svc;
            this._mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {

            DataResponse<Funcionario> responseFuncionario = await _Funcionario.GetAll();

            if (!responseFuncionario.HasSuccess)
            {
                ViewBag.Errors = responseFuncionario.Message;
                return View();
            }


            List<FuncionarioSelectViewModel> funcionario = _mapper.Map<List<FuncionarioSelectViewModel>>(responseFuncionario.Data);
            return View(funcionario);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FuncionariosInsertViewModel viewModel)
        {

            viewModel.Senha = viewModel.Senha.Hash();
            Funcionario Funcionario = _mapper.Map<Funcionario>(viewModel);

            Response response = await _Funcionario.Insert(Funcionario);

            if (response.HasSuccess)
                return RedirectToAction("Index");

            ViewBag.Errors = response.Message;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            SingleResponse<Funcionario> resoponseFuncionario = await _Funcionario.GetById(id);
            if (!resoponseFuncionario.HasSuccess)
            {

                return RedirectToAction("Index");
            }
            Funcionario funcionario = resoponseFuncionario.Item;
            FuncionarioUpdateViewModel updateViewModel = _mapper.Map<FuncionarioUpdateViewModel>(funcionario);
            return View(updateViewModel);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(FuncionarioUpdateViewModel viewModel)
        {
            viewModel.Senha = viewModel.Senha.Hash();
            Funcionario funcionario = _mapper.Map<Funcionario>(viewModel);
            Funcionario funcionarioUpdate = _Funcionario.GetById(funcionario.Id).Result.Item;
            funcionarioUpdate.Senha = funcionario.Senha;
            funcionarioUpdate.Nome = funcionario.Nome;
            funcionarioUpdate.Sobrenome = funcionario.Sobrenome;
            funcionarioUpdate.DataNascimento = funcionario.DataNascimento;
            funcionarioUpdate.Genero = funcionario.Genero;
            funcionarioUpdate.NivelDeAcesso = funcionario.NivelDeAcesso;
            Response response = await _Funcionario.Update(funcionarioUpdate);
            if (response.HasSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Errors = response.Message;
            return View(funcionario);
        }

        public IActionResult Delete()
        {
            return View();
        }

    }
}