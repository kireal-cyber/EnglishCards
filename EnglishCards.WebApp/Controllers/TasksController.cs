using System.Net.Http.Json;
using EnglishCards.WebApp.Models;
using EnglishCards.WebApp.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnglishCards.WebApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly HttpClient _apiClient;

        public TasksController(IHttpClientFactory httpClientFactory)
        {
            _apiClient = httpClientFactory.CreateClient("EnglishCardsApi");
        }


        public async Task<IActionResult> Index()
        {
            var apiTasks = await _apiClient.GetFromJsonAsync<List<TaskItemDto>>("api/tasks");

            
            var model = apiTasks?.Select(t => new TaskItemViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = (TaskStatusVm)t.Status,
                DeckId = t.DeckId,
                DeckName = t.Deck?.Name,
                AssignedUserId = t.AssignedUserId,
                AssignedUserName = t.AssignedUser?.Name
            }).ToList() ?? new List<TaskItemViewModel>();

            return View(model);
        }
        private async Task<List<DeckDto>> LoadDecksAsync()
        {
            var decks = await this._apiClient.GetFromJsonAsync<List<DeckDto>>("api/decks");
            return decks ?? new List<DeckDto>();
        }

        private List<SelectListItem> ToDeckSelectList(IEnumerable<DeckDto> decks, int? selectedId = null)
        {
            return decks
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name,
                    Selected = selectedId.HasValue && d.Id == selectedId.Value,
                })
                .ToList();
        }

        private List<SelectListItem> GetStatusSelectList(TaskStatusVm? selected = null)
        {
            return Enum.GetValues(typeof(TaskStatusVm))
                .Cast<TaskStatusVm>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString(),
                    Selected = selected.HasValue && s == selected.Value,
                })
                .ToList();
        }
    
        public async Task<IActionResult> Create()
        {
            var decks = await this.LoadDecksAsync();
            ViewBag.Decks = this.ToDeckSelectList(decks);
            ViewBag.Statuses = this.GetStatusSelectList(TaskStatusVm.New);

            var model = new TaskItemViewModel
            {
                Status = TaskStatusVm.New,
            };

            return this.View(model);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItemViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var decks = await this.LoadDecksAsync();
                ViewBag.Decks = this.ToDeckSelectList(decks, model.DeckId);
                ViewBag.Statuses = this.GetStatusSelectList(model.Status);
                return this.View(model);
            }

            var dto = new TaskItemDto
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                Status = (int)model.Status,
                DeckId = model.DeckId,
                AssignedUserId = model.AssignedUserId,
            };

            var response = await this._apiClient.PostAsJsonAsync("api/tasks", dto);
            response.EnsureSuccessStatusCode();

            return this.RedirectToAction(nameof(this.Index));
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var apiTask = await this._apiClient.GetFromJsonAsync<TaskItemDto>($"api/tasks/{id}");
            if (apiTask == null)
            {
                return this.NotFound();
            }

            var model = new TaskItemViewModel
            {
                Id = apiTask.Id,
                Title = apiTask.Title,
                Description = apiTask.Description,
                DueDate = apiTask.DueDate,
                Status = (TaskStatusVm)apiTask.Status,
                DeckId = apiTask.DeckId,
                AssignedUserId = apiTask.AssignedUserId,
                DeckName = apiTask.Deck?.Name,
                AssignedUserName = apiTask.AssignedUser?.Name,
            };

            var decks = await this.LoadDecksAsync();
            ViewBag.Decks = this.ToDeckSelectList(decks, model.DeckId);
            ViewBag.Statuses = this.GetStatusSelectList(model.Status);

            return this.View(model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskItemViewModel model)
        {
            if (id != model.Id)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                var decks = await this.LoadDecksAsync();
                ViewBag.Decks = this.ToDeckSelectList(decks, model.DeckId);
                ViewBag.Statuses = this.GetStatusSelectList(model.Status);
                return this.View(model);
            }

            var dto = new TaskItemDto
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                Status = (int)model.Status,
                DeckId = model.DeckId,
                AssignedUserId = model.AssignedUserId,
            };

            var response = await this._apiClient.PutAsJsonAsync($"api/tasks/{id}", dto);
            response.EnsureSuccessStatusCode();

            return this.RedirectToAction(nameof(this.Index));
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            var apiTask = await this._apiClient.GetFromJsonAsync<TaskItemDto>($"api/tasks/{id}");
            if (apiTask == null)
            {
                return this.NotFound();
            }

            var model = new TaskItemViewModel
            {
                Id = apiTask.Id,
                Title = apiTask.Title,
                Description = apiTask.Description,
                DueDate = apiTask.DueDate,
                Status = (TaskStatusVm)apiTask.Status,
                DeckId = apiTask.DeckId,
                DeckName = apiTask.Deck?.Name,
                AssignedUserId = apiTask.AssignedUserId,
                AssignedUserName = apiTask.AssignedUser?.Name,
            };

            return this.View(model);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await this._apiClient.DeleteAsync($"api/tasks/{id}");
            response.EnsureSuccessStatusCode();

            return this.RedirectToAction(nameof(this.Index));
        }
        private async Task<List<UserDto>> LoadUsersAsync()
        {
            var users = await this._apiClient.GetFromJsonAsync<List<UserDto>>("api/users");
            return users ?? new List<UserDto>();
        }

        private List<SelectListItem> ToUserSelectList(IEnumerable<UserDto> users, int? selectedId = null)
        {
            var items = users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Name,
                    Selected = selectedId.HasValue && u.Id == selectedId.Value,
                })
                .ToList();

            items.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "(все пользователи)",
                Selected = !selectedId.HasValue,
            });

            return items;
        }
                
        public async Task<IActionResult> Assigned(int? userId, TaskStatusVm? status, string? sort)
        {
            
            var users = await this.LoadUsersAsync();
            ViewBag.Users = this.ToUserSelectList(users, userId);

           
            var url = "api/tasks/assigned";

            var query = new List<string>();
            if (userId.HasValue)
            {
                query.Add($"userId={userId.Value}");
            }

            if (status.HasValue)
            {
                query.Add($"status={(int)status.Value}");
            }

            if (!string.IsNullOrEmpty(sort))
            {
                query.Add($"sort={sort}");
            }

            if (query.Any())
            {
                url += "?" + string.Join("&", query);
            }

            var apiTasks = await this._apiClient.GetFromJsonAsync<List<TaskItemDto>>(url);

            var model = apiTasks?.Select(t => new TaskItemViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = (TaskStatusVm)t.Status,
                DeckId = t.DeckId,
                DeckName = t.Deck?.Name,
                AssignedUserId = t.AssignedUserId,
                AssignedUserName = t.AssignedUser?.Name,
            }).ToList() ?? new List<TaskItemViewModel>();

            ViewBag.SelectedStatus = status;
            ViewBag.Sort = sort;

            return this.View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, TaskStatusVm status, string? returnUrl)
        {
        
            var patchUrl = $"api/tasks/{id}/status?status={(int)status}";

            var request = new HttpRequestMessage(HttpMethod.Patch, patchUrl);
            var response = await this._apiClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(nameof(this.Assigned));
        }



    }

}
