using Npgsql;
using System;
using Microsoft.EntityFrameworkCore;
using Coink.Models;
using CoinkApi.Data;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://localhost:5000");

var saludo = "Hola mundo";
var strConn = "Host=localhost;Username=coinkuser;Password=una456w34;Database=coinkdata";

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(strConn));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

string GenerarStringAleatorio(int longitud) {
  const string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  Random random = new Random();
  char[] caracteresGenerados = new char[longitud];

  for (int i = 0; i < longitud; i++) {
    caracteresGenerados[i] = caracteresPermitidos[random.Next(caracteresPermitidos.Length)];
  }

  return new string(caracteresGenerados);
}//fin de generar token para user

async Task<ApiResponse> ListarDepartamentos(AppDbContext db, int? id_pais){
  int pais = id_pais ?? 0;
  var response = new ApiResponse { Code = 200, Message = "Datos ok", Data = new List<object>() };
  // Traer todos los departamentos o filtrar por id_pais
  var departamentos = pais == 0 ? await db.Departamentos.ToListAsync()  : await db.Departamentos.Where(d => d.IdPais == pais).ToListAsync();
  if (departamentos == null || !departamentos.Any()){
    response.Code=404;
    response.Message = "No se encontraron departamentos con ese id";    
    return response;
  }

  foreach (var departamento in departamentos) {
    var departamentoData = new { id = departamento.Id, nombre = departamento.Nombre, pais=departamento.IdPais };
    response.Data.Add(departamentoData);    
  }
  
  return response;    
}//fin ed Listar departamentos

async Task<ApiResponse> ListarMunicipios(AppDbContext db, int? id_departamento){
  int departamento = id_departamento ?? 0;
  var response = new ApiResponse { Code = 200, Message = "Datos ok", Data = new List<object>() };

  var municipios = departamento == 0 ? await db.Municipios.ToListAsync()  : await db.Municipios.Where(d => d.IdDepartamento == departamento).ToListAsync();
  if (municipios == null || !municipios.Any()){
    response.Code=404;
    if(departamento == 0){
      response.Message = "No se encontraron municipios";  
    }else{
      response.Message = "No se encontraron municipios para ese departamento";
    }
      
    return response;
  }

  foreach (var municipio in municipios) {
    var municipioData = new { id = municipio.IdMunicipio, nombre = municipio.Nombre, departamento=municipio.IdDepartamento };
    response.Data.Add(municipioData);    
  }
  
  return response; 

}//fin de Listar municipios

app.MapGet("/", () => {    
  return "API v 0.0.1";
}).WithName("greetings").WithOpenApi();

app.MapGet("/departamentos", async (AppDbContext db) =>{
  var _response = await ListarDepartamentos(db, null);
  return Results.Json(_response,statusCode: _response.Code);
}).WithOpenApi();

app.MapGet("/departamentos/{id_pais}", async (int id_pais, AppDbContext db) =>{
   
  var _response = await ListarDepartamentos(db, id_pais);
  return Results.Json(_response,statusCode: _response.Code);

}).WithOpenApi();


app.MapGet("/municipios", async (AppDbContext db) =>{
   
  var _response = await ListarMunicipios(db, null);
  return Results.Json(_response,statusCode: _response.Code);

}).WithOpenApi();

app.MapGet("/municipios/{id_departamento}", async (int id_departamento, AppDbContext db) =>{
   
  var _response = await ListarMunicipios(db, id_departamento);
  return Results.Json(_response,statusCode: _response.Code);

}).WithOpenApi();


app.MapGet("/list_users", async (AppDbContext db) =>{
   
  var response = new ApiResponse { Code = 200, Message = "Datos ok", Data = new List<object>() };

  var usuarios = await db.Usuarios.ToListAsync();
  if (usuarios == null || !usuarios.Any()){
    response.Code=404;    
    response.Message = "No se encontraron usuarios";
  }

  foreach (var usuario in usuarios) {
    var userData = new { id = usuario.IdMask, nombre = usuario.Nombre, telefono=usuario.Telefono, direccion=usuario.Direccion, municipio=usuario.Municipio };
    response.Data.Add(userData);    
  }

  return Results.Json(response,statusCode: response.Code);
}).WithOpenApi();


app.MapPost("/add_user", (Usuario usuario) =>{

    var response = new ApiResponse { Code = 500, Message = "Datos del usuario invalidos", Data = new List<object>() };
    
    if (usuario == null) {
      return Results.Json(response,statusCode: 500);
    }

    string nombre2 = Uri.EscapeDataString(usuario.Nombre);
    string IdMask = GenerarStringAleatorio(16);

    using (var conn = new NpgsqlConnection(strConn)){
      conn.Open();
      using (var cmd = new NpgsqlCommand("SELECT insertar_usuario(@id_mask, @nombre, @telefono, @direccion, @municipio)", conn)) {
         
        cmd.Parameters.AddWithValue("id_mask", IdMask);
        cmd.Parameters.AddWithValue("nombre", usuario.Nombre);
        cmd.Parameters.AddWithValue("telefono", usuario.Telefono);
        cmd.Parameters.AddWithValue("direccion", usuario.Direccion);
        cmd.Parameters.AddWithValue("municipio", usuario.Municipio);

        var result = cmd.ExecuteScalar();

        // if (result.ToString().Contains("Error")) {
        if (result == null || result.ToString().Contains("Error")) {
          response.Code = 500;
          response.Message = "Sucedio un error al insertar el usuario";          
          //response.Data.Add(result.ToString());
          Console.WriteLine("jueputaaa");
          // Console.WriteLine(result.ToString());
        }else{
          response.Code = 200;
          response.Data.Clear();
          response.Message = "Usuario insertado correctamente";            
        }
      
      }

    }

    return Results.Json(response,statusCode: response.Code);

}).WithName("add_user").WithOpenApi();;


app.Run();



