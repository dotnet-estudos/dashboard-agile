/* 
 * Atributo para Extensao ActionResult gerando uma Mensagem em uma View 
 * 
 * Necessario Framework Bootstrap para exibição das Mensagens (Importante)
 * 
 * 
 * Visite nossa página http://www.codigoexpresso.com.br
 * 
 */

using System.Web.Mvc;

/// <summary>
/// Classe para exibição de mensagens Pop-up
/// </summary>
public static class ActionResultExtensions
{
    /// <summary>
    /// Redireciona para uma ActionResult retornando uma mensagem de confirmação para a View
    /// </summary>
    /// <param name="actionResult"></param>
    /// <param name="mensagem">Mensagem a ser exibida</param>
    /// <param name="titulo">titulo a ser exibido, sendo omitido apresenta defaut 'Atenção'</param>
    /// <returns></returns>
    public static ActionResult Mensagem(this ActionResult actionResult, string mensagem, string titulo = "Atenção")
    {
        return new TempDataActionResult(actionResult, mensagem, titulo);
    }

    public static ActionResult MensagemSucesso(this ActionResult actionResult, string mensagem)
    {
        return new TempDataActionResult(actionResult, mensagem, "", "sucesso");
    }

    public static ActionResult MensagemErro(this ActionResult actionResult, string mensagem)
    {
        return new TempDataActionResult(actionResult, mensagem, "", "erro");
    }
}

public static class PartialViewResultExtensions
{
    /// <summary>
    /// Redireciona para uma ActionResult retornando uma mensagem de confirmação para a View
    /// </summary>
    /// <param name="actionResult"></param>
    /// <param name="mensagem">Mensagem a ser exibida</param>
    /// <param name="titulo">titulo a ser exibido, sendo omitido apresenta defaut 'Atenção'</param>
    /// <returns></returns>
    public static PartialViewResult MensagemPartial(this ActionResult actionResult, string mensagem, string titulo = "Atenção")
    {
        return new TempDataPartialViewResult(actionResult, mensagem, titulo);
    }
}

public class TempDataActionResult : ActionResult
{
    private readonly ActionResult _actionResult;
    private readonly string _mensagem;
    private readonly string _titulo;
    private readonly string _tipo;

    public TempDataActionResult(ActionResult actionResult, string Mensagem, string Titulo)
    {
        _actionResult = actionResult;
        _mensagem = Mensagem;
        _titulo = Titulo;
    }

    public TempDataActionResult(ActionResult actionResult, string mensagem, string titulo, string tipo)
    {
        _actionResult = actionResult;
        _mensagem = mensagem;
        _titulo = titulo;
        _tipo = tipo;
    }

    public override void ExecuteResult(ControllerContext context)
    {
        context.Controller.TempData["Mensagem"] = _mensagem;
        context.Controller.TempData["Titulo"] = _titulo;
        context.Controller.TempData["Tipo"] = _tipo;
        _actionResult.ExecuteResult(context);
    }
}



public class TempDataPartialViewResult : PartialViewResult
{
    private readonly ActionResult _actionResult;
    private readonly string _mensagem;
    private readonly string _titulo;

    public TempDataPartialViewResult(ActionResult actionResult, string Mensagem, string Titulo)
    {
        _actionResult = actionResult;
        _mensagem = Mensagem;
        _titulo = Titulo;
    }

    public override void ExecuteResult(ControllerContext context)
    {
        context.Controller.TempData["Mensagem"] = _mensagem;
        context.Controller.TempData["Titulo"] = _titulo;
        _actionResult.ExecuteResult(context);
    }
}