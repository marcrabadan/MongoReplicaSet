using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoReplicaSet.WebApp.Data.Contexts;
using MongoReplicaSet.WebApp.Data.Entities;
using MongoReplicaSet.WebApp.Models;

namespace MongoReplicaSet.WebApp.Pages
{
    public class TweetsModel : PageModel
    {
        private readonly SocialContext _socialContext;

        public TweetsModel(SocialContext socialContext)
        {
            _socialContext = socialContext;
            Tweets = new List<Tweet>();
        }

        public IEnumerable<Tweet> Tweets { get; private set; }

        [BindProperty]
        public TweetModel Tweet { get; set; }

        public async Task OnGetAsync()
        {
            using (var cursor = await _socialContext.Tweets.GetAsync(c => true).ConfigureAwait(false))
            {
                while (await cursor.MoveNextAsync().ConfigureAwait(false))
                {
                    Tweets = Tweets.Concat(cursor.Current);
                }
            }
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
            
            await _socialContext.Tweets
                .DoTransactionAsync((cancellationToken) => _socialContext.Tweets.AddAsync(entity, cancellationToken))
                .ConfigureAwait(false);
            
            return RedirectToPage("/Tweets");
        }
    }
}