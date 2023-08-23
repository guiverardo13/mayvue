const API_URL = "https://localhost:49766/";

async function api_getAll() {
  let response;

  try {
    response = await fetch(API_URL + "api/Movies");

    if (!response.ok) {
      throw new Error('Network response was not successful');
    }
      return response.json();

  } catch (error) {
    console.log(error);

    return null;
  }
}

async function api_getMovieById(id) {
  try {
    const response = await fetch(API_URL + "api/Movies" + `/${id}`);

    if (!response.ok) {
      throw new Error('Network response was not successful');
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error('An error occurred:', error);
    return null;
  }
}

async function api_post(payload) {
  try {
    const response = await fetch(API_URL + "api/Movies", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json", 
      },
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      console.log("Request failed:", response.status, response.statusText);
      return null; // Return null on failure
    }

    const responseData = await response.json(); // Parse the response JSON
    return responseData.id; // Return the new movie ID
  } catch (error) {
    console.log("Error:", error.message);
    return null;
  }
}

async function api_put(payload) {
  try {
    const response = await fetch(API_URL + "api/Movies" + `/${payload.id}`, {
      method: "PUT",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      return { success: false, error: "Failed to update the movie." };
    }
    
    const responseData = await response.json();

    return { success: true, data: responseData };
  } catch (error) {
    console.error(error);
    return { success: false, error: "An error occurred while updating the movie." };
  }
}

async function api_delete(id) {
  try {
    const response = await fetch(API_URL + `api/Movies/${id}`, { method: "DELETE" });

    if (!response.ok) {
      return { success: false };
    }

    return { success: true };
  } catch (error) {
    console.log(error);
    return { success: false };
  }
}

export { api_getAll, api_getMovieById ,api_post, api_put, api_delete };
