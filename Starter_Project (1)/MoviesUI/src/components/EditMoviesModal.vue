<template>
<button @click="openModal">Edit Prize</button>

  <div class="bg-modal" v-show="isModalOpen" ref="editModalRef">
    <div class="modal-content">
      <div id="close" @click="closeModal">+</div>
      <img src="public\logo.jpg" alt="logo" />
      <h2>Edit Movie</h2>
      <form @submit.prevent="editMovie">
        <input type="text" placeholder="Name" maxlength="50" v-model="newMovie.name" required />
        <textarea id="description" maxlength="500" placeholder="Description" v-model="newMovie.description"></textarea>
        <input type="text" placeholder="Release Year" maxlength="4" v-model="newMovie.releaseYear" required/>
        <div class="wrapper">
          <label for="academyAward">Academy Award: </label> 
          <input type="checkbox" id="academyAward" v-model="newMovie.academyAward" />
        </div>
        <input type="text" placeholder="DirectorId" v-model="newMovie.directorId" />
        <button type="submit">Submit</button>
      </form>
    </div>
  </div> 
</template>

<script>
import { api_getMovieById, api_put } from '../api.js';

export default {
  name: "EditMoviesModal",
  props: ['currentMovie'],
  data() {
    return {
      isModalOpen: false,
      selectedOption:'',
      newMovie: {
        "name": '',
        "description": '',
        "releaseYear": '',
        "academyAward": false,
        "directorId": null
      }
    };
  },
    methods: {
       async openModal(movieId) {
        this.isEditModalOpen = true;

        this.newMovie = { ...prizeData };
      
    },

        async editMovie() {
      try {
        const success = await api_put(this.currentMovie.ID, this.newMovie);

        if (success) {
          this.$emit('movie-edited'); 
          this.closeModal();
        } else {
          console.error("Failed to update the movie.");
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
		closeModal() {
      this.resetForm();
      this.isModalOpen = false;
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

</style>