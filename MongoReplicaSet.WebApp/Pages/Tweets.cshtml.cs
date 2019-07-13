using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MongoReplicaSet.WebApp.Data.Contexts;
using MongoReplicaSet.WebApp.Data.Entities;
using MongoReplicaSet.WebApp.Models;

namespace MongoReplicaSet.WebApp.Pages
{
    public class TweetsModel : PageModel
    {
        private readonly SocialContext _socialContext;
        private readonly ILogger<TweetsModel> _logger;

        public TweetsModel(SocialContext socialContext, ILogger<TweetsModel> logger)
        {
            _socialContext = socialContext;
            _logger = logger;
            Tweets = new List<Tweet>();
        }

        public IEnumerable<Tweet> Tweets { get; private set; }

        [BindProperty]
        public TweetModel Tweet { get; set; }

        public async Task OnGetAsync()
        {
            _logger.LogTrace("Start");
            try
            {
                var tweets = new List<Tweet>();
                
                using (var cursor = await _socialContext.Tweets.GetAsync(c => true))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        tweets.AddRange(cursor.Current);
                    }
                }
                Tweets = tweets;
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, ex.Message);
            }           

            _logger.LogTrace("End");
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(Tweet?.Message))
            {
                return Page();
            }
            
            var entity = new Tweet
            {
                Id = Guid.NewGuid(),
                Message = Tweet.Message
            };

            _logger.LogTrace("Start..");
            
            await _socialContext.Tweets
                .DoTransactionAsync((cancellationToken) => _socialContext.Tweets.AddAsync(entity, cancellationToken));
            
            return RedirectToPage("/Tweets");
        }
    }
}