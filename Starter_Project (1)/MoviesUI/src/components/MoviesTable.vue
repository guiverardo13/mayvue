<template>
  <header>
    <button v-on:click.prevent="openModal">Add Movie</button>
  </header>
  <div id="moviesTable">
    <table class="table table-dark table-striped table-hover">
      <thead class="thead-light">
        <tr>
          <th>Name</th>
          <th>Description</th>
          <th>Release Year</th>
          <th>Academy Award</th>
          <th>Director ID</th>
          <th style="min-width: 120px" scope="col"></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="movie in movies" :key="movie.ID">
          <td @click="getMovieById(movie.id)" class="clickable">
            {{ movie.name }}
          </td>
          <td>{{ movie.description }}</td>
          <td>{{ movie.releaseYear }}</td>
          <td>{{ movie.academyAward }}</td>
          <td>{{ movie.directorId }}</td>
          <td>
            <span class="clickable m-2" @click="openEditModal(movie.ID)">
              <font-awesome-icon icon="fa-solid fa-pen-to-square" />
            </span>
            <span class="clickable m-2">
              <font-awesome-icon icon="fa-solid fa-trash" />
            </span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div class="selected-movie-modal" v-if="selectedMovie">
    <div class="selected-movie-box">
      <img src="public\logo.jpg" alt="logo" />
      <div id="close" @click="closeSelectedMovie">+</div>
      <h2>Selected Movie Details</h2>
      <p class="selected-movie-name"><strong>{{ selectedMovie.name }}</strong></p>
      <p><strong>Description:</strong> {{ selectedMovie.description }}</p>
      <p><strong>Release Year:</strong> {{ selectedMovie.releaseYear }}</p>
    </div>
  </div>  
  <div class="bg-modal" v-show="isModalOpen">
    <div class="modal-content">
      <div id="close" @click="closeModal">+</div>
      <img src="public\logo.jpg" alt="logo" />
      <h2>Add Movie</h2>
      <form @submit.prevent="addMovie">
        <input type="text" placeholder="Name" maxlength="50" v-model="newMovie.name" required />
        <textarea id="description" maxlength="500" placeholder="Description" v-model="newMovie.description"></textarea>
        <input type="text" placeholder="Release Year" maxlength="4" v-model="newMovie.releaseYear" required/>
        <div class="wrapper">
          <label for="academyAward">Academy Award: </label> 
          <input type="checkbox" id="academyAward" v-model="newMovie.academyAward" />
        </div>
        <input type="text" placeholder="DirectorId" v-model="newMovie.directorId" integer/>
        <button type="submit">Submit</button>
      </form>
    </div>
  </div>
  <div class="success-message" v-if="showSuccess">
    <div class="sucess-message-content">
      <div class="message">
        <span class="text text-1">Success!</span> <br>
        <span class="text text-2">Movie was added to your collection</span>
      </div>
        <div class="close-message" @click="showSuccess = false">+</div>
    </div>
    <EditMoviesModal ref="editModalRef" :currentMovie="selectedMovie" v-if="isEditModalOpen" @closeModal="closeEditModal" @movie-edited="handleMovieEdited" />
  </div>
</template>

<script>
import { api_getAll, api_getMovieById, api_post, api_put, api_delete } from "../api.js";
import EditMoviesModal from './EditMoviesModal.vue';

export default {
  
  components: {
    EditMoviesModal
  },

	props: [],
	data() {
		return {
			movies: [],
			selectedMovie: null,
      localMovie: {...this.movie},
			isModalOpen: false,
      isEditModalOpen: false,
      showSuccess: false,
			newMovie: 
          {
            "name": '',
            "description": '',
            "releaseYear": '',
            "academyAward": false,
            "directorId": null
          }
		}
	},

	async mounted() {
		this.getMovies();
		this.getMovieById();
	},

	methods: {
		async getMovies() {
			 try {
      			let response = await api_getAll(); 

      				if (response === null) {
       					 console.log("There was an error loading the list of movies.");
      				} else {
       					 this.movies = response; 
      				}
    		} catch (error) {
      				console.error("An error occurred:", error);
    		}
		},

		async getMovieById(movieId) {
    		try {
        		let response = await api_getMovieById(movieId); 

        		if (response === null) {
            		console.log("There was an error loading the movie.");
        		} else {
            		this.selectedMovie = response; 
        		}
    		} catch (error) {
        		console.error("An error occurred:", error);
    		}
		},

	async addMovie() {
    try {
    // Checking if movie already exists
    const movieWithSameNameExists = this.movies.some(movie => movie.name.toLowerCase() === this.newMovie.name.toLowerCase());

    if (movieWithSameNameExists) {
      console.error("Movie with the same ID or name already exists.");
      alert("This movie already exists in your collection. Try again!");
      this.resetForm();
      window.location.reload(); // added this because alert was requiring user to refresh the page to add new movie
      return; 
    }
      const success = await api_post(this.newMovie);
      //check if movie exists
      if (success) {
        this.movies.push(this.newMovie);
        
        this.showSuccessMessage();
        this.resetForm();
        this.closeModal();
      } else {
        console.error("Failed to save the movie to the database.");
      }
    } catch (error) {
      console.error("An error occurred:", error);
    }
  },
  
		resetForm() {
      		this.newMovie = { 
        		name: '',
       			description: '',
        		releaseYear: '',
        		academyAward: false,
        		directorId: ''
      		};
		},

   openEditModal(movieId) {
    if (this.$refs.editModalRef) {
      this.$refs.editModalRef.openModal(movieId);
      this.isEditModalOpen = true; // Set the modal open state to true
    } else {
      console.error("EditMovieModal ref not found.");
    }
    },

    closeEditModal() {
      this.isEditModalOpen = false; // Set the modal open state to false
    },

		openModal() {
			this.isModalOpen = true;
		},

		closeModal() {
      this.resetForm();
      this.isModalOpen = false;
    },

		closeSelectedMovie() {
			this.selectedMovie = false;
		},
    showSuccessMessage() {
      this.showSuccess = true;
      setTimeout(() => {
      this.showSuccess = false;
    }, 4000);
  }
  }
}
</script>

<style>
.bg-modal {
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.7);
  position: fixed;
  top: 0;
  left: 0;
  z-index: 1;
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  width: 80%;
  max-width: 380px; 
  height: 480px;
  background-color: white;
  padding: 20px; 
  border-radius: 4px;
  text-align: center;
  align-items: center;
  position: relative;
}

.selected-movie-modal {
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.7);
  position: fixed;
  top: 0;
  left: 0;
  z-index: 1;
  display: flex;
  justify-content: center;
  align-items: center;
}

.selected-movie-box {
  width: 80%; 
  max-width: 380px;
  height: 480px;
  background-color: white;
  padding: 20px; 
  border-radius: 4px;
  text-align: center;
  align-items: center;
  position: relative;
  display: flex;
  justify-content: space-evenly;
  flex-direction: column;
}

.bg-modal h2 {
  font-size: 25px;
}

.selected-movie-box h2 {
  font-size: 25px;
}

.selected-movie-box > p { 
  margin: 10px;
  font-size: 15px;
}

.success-message {
   position: absolute;
   top:10px;
   right: 15px;
   border-radius: 12px;
   background: white;
   padding: 20px 35px 20px 25px;
   box-shadow: 0 5px 10px rgba(0, 0, 0, 0.1);
   border-left: 6px solid rgb(63, 235, 63)
}

.success-message-content {
  display: flex;
  align-items: center;
}

.message .text{
font-size: 20px;
font-weight: 600;
color: #5e5d5d;
}

.message .text.text-1{
  font-weight: 600;
  color: #333;
}

.success-message .close-message{
  position: absolute;
  top: 0px;
  font-size: 25px;
  right: 15px;
  cursor: pointer;
  transform: rotate(45deg);
}

.success-message .close-message:hover {
  opacity: 1;
}

.success-message-content .message {
  display: flex;
  flex-direction: column;
}

.slide-right-up {
  transform: translateY(0);
  transition: right 0.5s ease-in-out, transform 0.5s ease-in-out;
}

#close {
  position: absolute;
  top: 0;
  height: 30px;
  right: 0;
  font-size: 50px;
  transform: rotate(45deg);
  cursor: pointer;
}

.wrapper {
  display: inline-block;
}

#academyAward {
  margin-left: 10px;
}

#selected-movie-button {
  width: 80px;
  font-size: 15px;
}

form > input {
  width: 220px;
  display: block;
  margin: 15px auto;
}

button:hover {
  background-color: #c9c7c7;
}

form > textarea {
  width: 220px;
  display: block;
  margin: 15px auto;
}

img {
  width: 80px;
  border-radius: 250px;
  margin-bottom: 10px;
}

.clickable {
  cursor: pointer;
  user-select: none;
}

.selected-movie-box .selected-movie-name {
  font-size: 25px;
}

header {
  display: flex;
  margin-bottom: 10px;
}

section > button {
  width: 100px;
  font-size: 15px;
}

header > button {
  width: 90px;
  font-size: 15px;
  transition: 0.3s;
  border-radius: 6px;
}

.clickable:hover {
  color: rgb(104, 104, 252);
}
</style>
