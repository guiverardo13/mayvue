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

async function api_getMovieById(movieId) {
  let response;

  try {
    response = await fetch(API_URL + "api/Movies" + `/${movieId}`);

    if (!response.ok) {
      throw new Error('Network response was not successful');
    }
      return response.json();

  } catch (error) {
    console.log(error);

    return null;
  }
}

async function api_post(payload) {
  try {
    const response = await fetch(API_URL + "api/Movies", {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json", // Make sure this header is set
      },
      body: JSON.stringify(payload),
    });

    if (!response.ok) {
      console.log("Request failed:", response.status, response.statusText);
      return false;
    }

    return true;
  } catch (error) {
    console.log("Error:", error.message);
    return false;
  }
}

async function api_put(payload) {
  let response;
  try {
    response = await fetch(API_URL + "api/Movies" + `/${payload.id}`, {
      method: "PUT",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(payload),
    });
  } catch (error) {
    console.log(error);
  } finally {
    if (!response || !response.ok) {
      return false;
    } else return true;
  }
}

async function api_delete(id) {
  let response;
  try {
    response = await fetch(API_URL + `api/Movies/${id}`, { method: "DELETE" });
  } catch (error) {
    console.log(error);
  } finally {
    if (!response || !response.ok) {
      return false;
    } else return true;
  }
}

export { api_getAll, api_getMovieById ,api_post, api_put, api_delete };
