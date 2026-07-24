<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewSwitcher.ascx.cs" Inherits="Lexus2._0.ViewSwitcher" %>
<div id="viewSwitcher">
    <%: CurrentView %> view | <a href="<%: SwitchUrl %>" data-ajax="false">Switch to <%: AlternateView %></a>


    <table class="table table-bordered">
        <thead>
            <tr>
                <th scope="col" style="background-color:#861339; color:white; text-align:center">Tipo de licencia</th>
                <th scope="col" style="background-color:#861339; color:white; text-align:center">Nueva</th>
                <th scope="col" style="background-color:#861339; color:white; text-align:center">Canje</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>A / Servicio Transporte Público y Taxi</td>
                <td>$1,242.00</td>
                <td>1,242.00</td>
            </tr>
            <tr>
                <td>B / Vehículos de Transporte Público de Carga y Particular</td>
                <td>$2,207.00</td>
                <td>$1,104.00</td>
            </tr>
            <tr>
                <td>C / Vehículos Particulares que no excedan las 3.5 toneladas.</td>
                <td>$1,931.00</td>
                <td>$966.00</td>
            </tr>
            <tr>
                <td>D / Vehículos de 2 o 3 ruedas y Cuatrimoto.</td>
                <td>$1,380.00</td>
                <td>$828.00</td>
            </tr>
            <tr>
                <td>Permiso para menor de edad y extranjero (Para mayores de 16 años y menores de 18 años).</td>
                <td>$1,104.00</td>
            </tr>
              <tr>
                <td>Duplicado por robo o extravío (Aplica a todas las licencias vigentes).</td>
                <td>$552.00</td>
            </tr>
        </tbody>
    </table>

    <div class="container table">
        <div class="row">
            <div class="col">Tipo de licencia</div>
            <div class="col">Nueva</div>
            <div class="col">Canje</div>
        </div>

        <div class="row">
            <div class="col">Licencia Tipo A / Servicio Transporte Público y Taxi</div>
            <div class="col">$1,242.00</div>
            <div class="col">$276</div>
        </div>
        <div class="row">
            <div class="col">Licencia tipo B / Vehículos de Transporte Público de Carga y Particular</div>
            <div class="col">$2,207.00</div>
            <div class="col">$1,104.00</div>
        </div>
        <div class="row">
            <div class="col">Licencia Tipo C / Vehículos Particulares que no excedan las 3.5 toneladas.</div>
            <div class="col">$1,931.00</div>
            <div class="col">$966.00</div>
        </div>
        <div class="row">
            <div class="col">Licencia tipo D / Vehículos de 2 o 3 ruedas y Cuatrimoto.</div>
            <div class="col">$1,380.00</div>
            <div class="col">$828.00</div>
        </div>
        <div class="row">
            <div class="col">Permiso para menor de edad y extranjero (Para mayores de 16 años y menores de 18 años).</div>
            <div class="col">$1,104.00</div>
        </div>

        <div class="row">
            <div class="col">Duplicado por robo o extravío (Aplica a todas las licencias vigentes).</div>
            <div class="col">$552.00</div>
        </div>
    </div>
</div>
