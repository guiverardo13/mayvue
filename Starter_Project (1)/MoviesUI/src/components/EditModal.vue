<template>
    <div class="edit-movie-modal"  v-if="isEditModalOpen">
        <div class="edit-modal-content">
            <div id="close" @click="closeEditModal">+</div>
            <img src="public\logo.jpg" alt="logo" />
            <h2>Edit Movie</h2>
            <form>
                <input type="text" placeholder="Name" maxlength="50" v-model="editedMovie.name"/>
                <textarea id="description" maxlength="500" placeholder="Description" v-model="editedMovie.description"></textarea>
                <input type="text" placeholder="Release Year" maxlength="4" v-model="editedMovie.releaseYear">
                <div class="wrapper">
                    <label for="academyAward">Academy Award: </label> 
                    <input type="checkbox" id="academyAward" v-model="editedMovie.academyAward"/>
                </div>
                <input type="text" placeholder="DirectorId" v-model="editedMovie.directorId">
                <button type="submit">Submit</button>
            </form>
        </div>
    </div>
    <div class="delete-modal" v-if="isDeleteModalOpen">
      <div class="delete-modal-content">
        <div>
          <h2>Are you sure you want to delete <br> the movie "{{ currentDeleteMovie.name }}" ?</h2>
        </div>
        <div class="buttons">
          <button class="yes" @click="confirmDelete">Yes</button>
          <button class="no" @click="closeDeleteModal"><strong>Cancel</strong></button>
        </div>
      </div>
    </div>
    <div class="delete-message" v-if="showMessage">
    <div class="delete-message-content">
      <div class="message">
        <span class="text text-1">Success!</span> <br>
        <span class="text text-2">Movie was deleted from your collection</span>
      </div>
        <div class="close-message" @click="showMessage = false">+</div>
      </div>
    </div>
</template>

<script>
import { api_getAll, api_getMovieById } from '../api';
export default {
    emits: ['closeEditModal', 'closeDeleteModal', 'confirmDelete', 'showDeleteMessage'],
    props: {
        isEditModalOpen: Boolean,
        currentMovie: Object,
        isDeleteModalOpen: Boolean,
        currentDeleteMovie: Object
    },

    data() {
		  return {
            showMessage: false,
            editedMovie: {
                name: '',
                description: '',
                releaseYear: '',
                academyAward: false,
                directorId: null
            }
        }
    },
    watch: {
        currentMovie: {
            handler(newMovieData) {
                this.editedMovie = { ...newMovieData };
            },
            immediate: true
        },
    },

    methods: {

        closeEditModal() {
            this.$emit('closeEditModal');
        },

        closeDeleteModal() {
          this.$emit('closeDeleteModal');
        },

        confirmDelete() {
          const confirmed = true;
    
          if (confirmed) {
            this.$emit('confirmDelete', this.currentDeleteMovie.id);
            this.closeDeleteModal();
            this.showDeleteMessage();
            api_getAll();
          }
      },

        showDeleteMessage() {
          this.showMessage = true;
          setTimeout(() => {
          this.showMessage = false;
          }, 1500);
        }
      }
    }
    
</script>

<style>
.edit-movie-modal {
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

.edit-modal-content {
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

.delete-message {
   position: fixed;
   top: 12px;
   right: 15px;
   border-radius: 12px;
   background: white;
   padding: 20px 35px 20px 25px;
   box-shadow: 0 5px 10px rgba(0, 0, 0, 0.1);
   border-left: 6px solid rgb(230, 83, 83);
   border-bottom: 4px solid rgb(230, 83, 83);
}

.delete-message-content {
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

.success-message .close-message:hover {
  opacity: 1;
}

.delete-message .close-message{
  position: absolute;
  top: 0px;
  font-size: 25px;
  right: 15px;
  cursor: pointer;
  transform: rotate(45deg);
}

.delete-modal {
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

.delete-modal-content {
  width: 350px;
  max-height: 80vh;
  background-color: white;
  padding: 20px; 
  border-radius: 4px;
  text-align: center;
  align-items: center;
  position: relative;
}

.delete-modal-content h2 {
  font-size: 18px;
}

.delete-modal-content {
  display: flex;
  flex-direction: column;
}

.buttons {
  display: flex;
  justify-content: space-between; 
  align-items: center; 
  margin-top: 20px; 
}

.yes {
  margin-right: 10px;
  width: 80px;
  border-radius: 5px;
}

.no {
  width: 80px;
  border-radius: 5px;
  background-color: rgb(230, 83, 83);
}

.no:hover {
  background-color: rgb(214, 57, 57);
}


.edit-movie-modal img {
  width: 80px;
  border-radius: 250px;
  margin-bottom: 10px;
}

.edit-movie-modal h2 {
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

form button {
  margin-top: 10px;
}

.clickable:hover {
  color: rgb(104, 104, 252);
}

</style>