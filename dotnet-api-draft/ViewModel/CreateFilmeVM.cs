﻿using System.ComponentModel.DataAnnotations;

namespace dotnet_api_draft.ViewModel;

public class CreateFilmeVM
{
[Required(ErrorMessage = "O campo Titulo é obrigatório.")]
public string? Titulo { get; set; }

[Required(ErrorMessage = "O campo Genero é obrigatório.")]
[StringLength(50, ErrorMessage = "O campo Genero deve ter no máximo 50 caracteres.")]
public string? Genero { get; set; }  
[Required(ErrorMessage = "O campo Duracao é obrigatório.")]
[Range(1, 300, ErrorMessage = "O campo Duracao deve ser um número entre 15 e 300.")]
[Display(Name = "Duração em minutos")]
public int Duracao { get; set; }
}
