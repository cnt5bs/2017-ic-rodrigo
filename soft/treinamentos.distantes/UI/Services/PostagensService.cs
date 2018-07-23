using App.Models;
using ORM.Conteudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Services
{
    public class PostService
    {
        
        public Post InsertPost(Post Post)
        {
            if (Post.pages == null)
            {
                PagesService pageService = new PagesService();
                Post.pages = new List<Pages>();
                Post.pages.Add(new Pages()
                {
                    name = "Orientacao",
                    type = "Orientacao",
                    text = "",
                    imagePaths = new List<string>(),
                    ordem = 1
                });
                Pages p = new Pages()
                {
                    name = "questionario-Orientacao",
                    type = "questionario",
                    text = "",
                    imagePaths = new List<string>(),
                    ordem = 0
                };
                Post.pages.Add(p);

            }
            return new PostagensDB().InsertPost(Post);
        }
        public Post findPostByID(string ID)=>findPostByID(ID, true);
        
        
        public Post findPostByID(string ID, bool listInactivePages)
        {
            PostagensDB PostRepository = new PostagensDB();
            Post post = PostRepository.FindPostByID(ID);
            if(post != null)
            {
                if (post.Orientacao != null)
                {
                    if (post.Orientacao[0].questionario != null)
                    {
                        if (post.Orientacao[0].questionario[0].perguntas == null)
                            post.Orientacao[0].questionario[0].perguntas = new List<Pergunta>();
                        List<Pergunta> perguntas = post.Orientacao[0].questionario[0].perguntas.Where(p => p.active == false).ToList();

                        foreach (var pergunta in perguntas)
                        {
                            post.Orientacao[0].questionario[0].perguntas.Remove(pergunta);
                        }
                    }
                }
                if (!listInactivePages)
                    post.pages = post.pages.Where(u => u.active == true).OrderByDescending(u => u.ordem).ToList();
            }
            return post;
        }
        public List<Post> ListActivePosts()=>new PostagensDB().ListActivePosts();


        public List<Post> ListActivePostsNotBind(string postID)
        {
            PostagensDB postsRepository = new PostagensDB();
            Post posts = postsRepository.FindPostByID(postID);
            if (posts.correlatedPosts == null)
                return postsRepository.ListActivePosts();
            return postsRepository.ListActivePosts().Where(p => !new PostagensDB().FindPostByID(postID).correlatedPosts.Select(po => po.ID).Contains(p.ID)).ToList();
        }

        public List<Post> ListActivePostsBind(string postID) => new PostagensDB().ListActivePosts().Where(p => new PostagensDB().FindPostByID(postID).correlatedPosts != null && new PostagensDB().FindPostByID(postID).correlatedPosts.Select(po => po.ID).Contains(p.ID)).ToList();

        public List<Post> ListNotActivePosts()=> new PostagensDB().ListNotActivePosts();
        
        public void UpdatePostByID(string ID, Dictionary<string, object> properties)
        {
            new PostagensDB().UpdatePostByID(ID, properties);
        }
        public void DeactivatePostByID(List<string> IDs)
        {
            PostagensDB PostRepository = new PostagensDB();
            Dictionary<string, object> properties = new Dictionary<string, object>() { { "active", "false" } };
            foreach (var id in IDs)
                PostRepository.UpdatePostByID(id, properties);
            
        }
    }
}