import api from './axios.js';

export async function  generateGeminiResponse(prompt) {
  try {
    const response =await api.post('/gemini/generate', { prompt });
    console.log(response);
    return response.data;
  }
  catch (error) {
    console.error("Error loading response" + error);
    return "Something went wrong";
  }

};
