<template>
  <div class="layout d-flex">
    <Sidebar />

    <div class="main-content flex-grow-1">
      <Navbar />
      <div class="content-wrapper p-4">
        <slot />

        <!-- Chatbot -->
        <div>
          <!-- Floating Chat Button -->
          <button class="btn btn-primary chatbot-btn" @click="openChatbot">
            💬
          </button>

          <!-- Chat Popup -->
          <transition name="fade">
            <div v-if="isOpen" class="chatbox d-flex flex-column">
              <div class="chat-header bg-primary text-white d-flex justify-content-between align-items-center p-2">
                <span>Chatbot</span>
                <button class="btn-close btn-close-white" @click="closeChatbot"></button>
              </div>

              <div class="chat-messages flex-grow-1 p-2" ref="chatContainer">
                <div v-for="(msg, index) in messages"
                     :key="index"
                     :class="['message', msg.sender]">
                  {{ msg.text }}
                </div>
              </div>

              <div class="input-group p-2">
                <input v-model="userInput"
                       @keyup.enter="sendMessage"
                       type="text"
                       class="form-control"
                       placeholder="Type a message..." />
                <button class="btn btn-primary" @click="sendMessage">Send</button>
              </div>
            </div>
          </transition>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import Sidebar from "../components/Sidebar.vue";
  import Navbar from "../components/Navbar.vue";
  import { ref, nextTick } from "vue";
  import { generateGeminiResponse } from "../services/geminiService";

  const isOpen = ref(false);
  const userInput = ref("");
  const messages = ref([]);
  const chatContainer = ref(null);

  function openChatbot() {
    isOpen.value = true;
  }

  function closeChatbot() {
    isOpen.value = false;
  }


  async function sendMessage() {
    if (!userInput.value.trim()) return;

    const text = userInput.value;
    messages.value.push({ text, sender: "user" });
    userInput.value = "";
    nextTick(() => scrollToBottom());

    const botResponse = await generateGeminiResponse(text);
    messages.value.push({ text: botResponse, sender: "bot" });
    nextTick(() => scrollToBottom());
  }

  function scrollToBottom() {
    const container = chatContainer.value;
    if (container) container.scrollTop = container.scrollHeight;
  }
</script>

<style scoped>
  .layout {
    min-height: 100vh;
    display: flex;
    background: linear-gradient(135deg, #e0f0ff, #ffffff);
  }

  .main-content {
    flex: 1;
    display: flex;
    flex-direction: column;
  }

  .content-wrapper {
    flex-grow: 1;
    padding: 2rem;
    background: transparent;
  }

  /* Chatbot Styles */
  .chatbot-btn {
    position: fixed;
    bottom: 20px;
    right: 20px;
    border-radius: 50%;
    width: 60px;
    height: 60px;
    font-size: 24px;
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1050;
  }

  .chatbox {
    position: fixed;
    bottom: 90px;
    right: 20px;
    width: 320px;
    max-height: 400px;
    display: flex;
    flex-direction: column;
    border-radius: 12px;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
    background: white;
    z-index: 1050;
    overflow: hidden;
  }

  .chat-header {
    font-weight: bold;
  }

  .chat-messages {
    overflow-y: auto;
    height: 250px;
    display: flex;
    flex-direction: column;
    gap: 5px;
  }

  .message.user {
    background-color: #0d6efd;
    color: white;
    align-self: flex-end;
    padding: 8px 12px;
    border-radius: 20px;
    max-width: 80%;
  }

  .message.bot {
    background-color: #f1f1f1;
    color: black;
    align-self: flex-start;
    padding: 8px 12px;
    border-radius: 20px;
    max-width: 80%;
  }

  /* Transition for popup */
  .fade-enter-active, .fade-leave-active {
    transition: opacity 0.3s;
  }

  .fade-enter-from, .fade-leave-to {
    opacity: 0;
  }
</style>
