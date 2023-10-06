using PretPark_Api_Opdracht.Models;
using PretPark_Api_Opdracht.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace W6API.Controller;

[Route("api/[controller]")]
[ApiController]
public class LikeController : ControllerBase
{
    private readonly PretparkContext _context;

    public LikeController(PretparkContext context)
    {
        _context = context;
    }


    [HttpPost("{id}")]
    public async Task<ActionResult<Attractie>> LikeAttractie(int id){
        if (_context.Attractie == null)
        {
            return NotFound();
        }
        var attractie = await _context.Attractie.Include("UserLikes").SingleAsync(a => a.Id == id);
        if (attractie == null)
        {
            return NotFound();
        }
        var currentUser = await _context.Gebruikers.SingleOrDefaultAsync(g => g.UserName == getSignedUser());
        if (currentUser == null)
        {
            return NotFound();
        }
        //Check if user has liked the attraction
        if(currentUser.LikedAttractions.Where(a => a.Id == attractie.Id).Count() > 0){
            attractie.UserLikes.Remove(currentUser);
            currentUser.LikedAttractions.Remove(attractie);
        }
        else{
            attractie.UserLikes.Add(currentUser);
            currentUser.LikedAttractions.Add(attractie);
        }
        await _context.SaveChangesAsync();
        return attractie;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attractie>>> GetLikedAttractions(){
        var currentUser = await _context.Gebruikers.Include("LikedAttractions").SingleOrDefaultAsync(g => g.UserName == getSignedUser());

        if(currentUser !=null){
            return currentUser.LikedAttractions.ToList();
        }
        return NotFound();
    }

    private string getSignedUser() {
        Request.Headers.TryGetValue("Authorization", out var headervalue);
        string cleanToken = headervalue.ToString().Substring(7);
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(cleanToken);
        var tokenS = jsonToken as JwtSecurityToken;
        string[] loggedInUserDisgusting = tokenS.Claims.ToList()[0].ToString().Split(": ");
        string loggedInUser = loggedInUserDisgusting[1];
        return loggedInUser;
    }




}
