<template>
  <div class="study-partner">
    <div class="content">
      <h1 class="timestamp">{{ date }}</h1>
      <h3>The LLM output will go here</h3>
    </div>
    <div class="ai-input">
      <input type="text" placeholder="What can I help you with today?" />
    </div>
  </div>
</template>

<script>
import { getTimeStampLongFormat } from '../helpers/DateHelper'

export default {
  data() {
    return {}
  },
  computed: {
    date() {
      return getTimeStampLongFormat()
    },
  },
}
</script>
<style scoped lang="scss">
.study-partner {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}
.content {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  color: $primary-text-light;
}
.timestamp {
  font-size: 3rem;
}
.ai-input {
  display: flex;
  justify-content: center;
  width: 100%;
  margin-top: 5rem;
}
.ai-input input {
  width: 80%;
  height: 1rem;
  border-radius: 10px;
  padding: 10px;
  font-size: 1rem;
  background-color: $primary-background-dark;
  border: 2px solid $primary-red-outline;
  color: $primary-text-light;
}
.ai-input input::placeholder {
  color: $primary-text-light;
  text-align: center;
}
.ai-input input:focus {
  outline: none;
}
</style>
