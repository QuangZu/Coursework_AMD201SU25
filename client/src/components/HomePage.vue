<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
    <!-- Hero Section -->
    <div class="flex items-center justify-center py-20">
      <div class="text-center max-w-4xl mx-auto px-4">
        <h1 class="text-6xl font-bold text-gray-800 mb-6">
          Shorten Your <span class="text-blue-600">URLs</span>
        </h1>
        <p class="text-gray-600 text-xl mb-12 leading-relaxed max-w-2xl mx-auto">
          Transform long, complex URLs into short, shareable links. Fast, reliable, and completely free.
        </p>

        <!-- URL Shortener Form -->
        <div class="bg-white rounded-2xl shadow-xl p-8 mb-8 max-w-3xl mx-auto">
          <h2 class="text-2xl font-bold text-gray-800 mb-6 text-left">
            Quick Shorten URL
          </h2>
          <form @submit.prevent="shortenUrl" class="space-y-6">
            <div class="flex flex-col md:flex-row gap-4">
              <div class="flex-1">
                <input
                  v-model="longUrl"
                  type="url"
                  placeholder="Enter your long URL here"
                  class="w-full px-6 py-4 text-lg border-2 border-gray-200 rounded-xl focus:border-blue-500 focus:outline-none transition-colors"
                  required
                />
              </div>
              <button
                type="submit"
                :disabled="isLoading"
                class="px-8 py-4 bg-blue-600 text-white font-semibold rounded-xl hover:bg-blue-700 focus:outline-none focus:ring-4 focus:ring-blue-200 transition-all duration-200 disabled:opacity-50"
              >
                {{ isLoading ? 'Shortening...' : 'Shorten URL' }}
              </button>
            </div>
          </form>

          <!-- Result Display -->
          <div v-if="shortUrl" class="mt-8 p-6 bg-green-50 border border-green-200 rounded-xl">
            <h3 class="text-lg font-semibold text-green-800 mb-3">Your shortened URL:</h3>
            <div class="flex items-center justify-between bg-white p-4 rounded-lg border">
              <a :href="shortUrl" target="_blank" class="text-blue-600 hover:text-blue-800 font-medium break-all">
                {{ shortUrl }}
              </a>
              <button
                @click="copyToClipboard"
                class="ml-4 px-4 py-2 bg-blue-600 text-white text-sm rounded-lg hover:bg-blue-700 transition-colors flex-shrink-0"
              >
                {{ copied ? 'Copied!' : 'Copy' }}
              </button>
            </div>
            <div class="mt-4 text-sm text-gray-600">
              <p><strong>Original URL:</strong> {{ longUrl }}</p>
              <p><strong>Clicks:</strong> 0 (Track your link performance)</p>
            </div>
          </div>

          <!-- Error Display -->
          <div v-if="error" class="mt-8 p-6 bg-red-50 border border-red-200 rounded-xl">
            <h3 class="text-lg font-semibold text-red-800 mb-2">Error:</h3>
            <p class="text-red-600">{{ error }}</p>
          </div>
        </div>

        <!-- URL shorten with custom alias -->
        <div class="bg-white rounded-2xl shadow-xl p-8 mb-16 max-w-3xl mx-auto">
          <h2 class="text-2xl font-bold text-gray-800 mb-6 text-left">Or shorten URL with custom alias</h2>
          <form @submit.prevent="shortenUrlWithAlias" class="space-y-6">
            <div class="flex flex-col gap-4">
              <div class="flex-1">
                <input
                  v-model="longUrl"
                  type="url"
                  placeholder="Enter your long URL here"
                  class="w-full px-6 py-4 text-lg border-2 border-gray-200 rounded-xl focus:border-blue-500 focus:outline-none transition-colors"
                  required
                />
              </div>
              <!-- System domain (fixed) -->
               <div class="flex items-center gap-2">
                <div class="bg-gray-50 px-6 py-4 rounded-xl border-2 border-gray-200">
                  <span class="text-gray-700 font-medium text-lg">{{ systemDomain }}/</span>
                </div>
                <div class="flex-1">
                  <input
                    v-model="customAlias"
                    type="text"
                    placeholder="Enter your custom alias here"
                    class="w-full px-6 py-4 text-lg border-2 border-gray-200 rounded-xl focus:border-blue-500 focus:outline-none transition-colors"
                    required
                  />
                </div>
               </div>

              <button
                type="submit"
                :disabled="isLoading"
                class="px-8 py-4 bg-blue-600 text-white font-semibold rounded-xl hover:bg-blue-700 focus:outline-none focus:ring-4 focus:ring-blue-200 transition-all duration-200 disabled:opacity-50"
              >
                {{ isLoading ? 'Shortening...' : 'Shorten URL' }}
              </button>
            </div>
          </form>
        </div>
        <!-- Features Section -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-8 mt-16">
          <div class="bg-white p-8 rounded-2xl shadow-lg hover:shadow-xl transition-shadow">
            <div class="text-blue-600 text-5xl mb-6">⚡</div>
            <h3 class="text-xl font-semibold mb-4">Lightning Fast</h3>
            <p class="text-gray-600">Shorten URLs instantly with our high-performance infrastructure and Redis caching.</p>
          </div>

          <div class="bg-white p-8 rounded-2xl shadow-lg hover:shadow-xl transition-shadow">
            <div class="text-blue-600 text-5xl mb-6">🔒</div>
            <h3 class="text-xl font-semibold mb-4">Secure & Reliable</h3>
            <p class="text-gray-600">Your links are protected with enterprise-grade security and 99.9% uptime guarantee.</p>
          </div>

          <div class="bg-white p-8 rounded-2xl shadow-lg hover:shadow-xl transition-shadow">
            <div class="text-blue-600 text-5xl mb-6">📊</div>
            <h3 class="text-xl font-semibold mb-4">Analytics & Tracking</h3>
            <p class="text-gray-600">Track clicks, monitor performance, and get detailed insights about your shortened URLs.</p>
          </div>
        </div>

        <!-- Stats Section -->
        <div class="mt-20 bg-white rounded-2xl shadow-xl p-8 max-w-4xl mx-auto">
          <h2 class="text-3xl font-bold text-center text-gray-800 mb-8">Why Choose Our URL Shortener?</h2>
          <div class="grid grid-cols-1 md:grid-cols-3 gap-8 text-center">
            <div>
              <div class="text-4xl font-bold text-blue-600 mb-2">1M+</div>
              <p class="text-gray-600">URLs Shortened</p>
            </div>
            <div>
              <div class="text-4xl font-bold text-blue-600 mb-2">99.9%</div>
              <p class="text-gray-600">Uptime Guarantee</p>
            </div>
            <div>
              <div class="text-4xl font-bold text-blue-600 mb-2">&lt;100ms</div>
              <p class="text-gray-600">Average Response Time</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'HomePage',
  data() {
    return {
      longUrl: '',
      shortUrl: '',
      customAlias: '',
      isLoading: false,
      copied: false,
      error: '',
      systemDomain: 'http://localhost:8000' // Fixed system domain - matches your Ocelot Gateway
    }
  },
  methods: {
    async shortenUrl() {
      if (!this.longUrl.trim()) {
        this.error = 'Please enter a valid URL'
        return
      }

      this.isLoading = true
      this.error = ''
      this.shortUrl = ''

      try {
        // Call your URL shortening API through Ocelot Gateway
        const response = await fetch('http://localhost:8000/shorten', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            longUrl: this.longUrl
          })
        })

        if (!response.ok) {
          throw new Error('Failed to shorten URL')
        }

        const data = await response.json()
        this.shortUrl = data.shortUrl || `${this.systemDomain}/${data.shortCode}`

      } catch (error) {
        console.error('Error shortening URL:', error)
        this.error = 'Failed to shorten URL. Please try again.'
      } finally {
        this.isLoading = false
      }
    },

    async copyToClipboard() {
      try {
        await navigator.clipboard.writeText(this.shortUrl)
        this.copied = true
        setTimeout(() => {
          this.copied = false
        }, 2000)
      } catch (error) {
        console.error('Failed to copy to clipboard:', error)
        // Fallback for older browsers
        const textArea = document.createElement('textarea')
        textArea.value = this.shortUrl
        document.body.appendChild(textArea)
        textArea.select()
        document.execCommand('copy')
        document.body.removeChild(textArea)
        this.copied = true
        setTimeout(() => {
          this.copied = false
        }, 2000)
      }
    },

    async shortenUrlWithAlias() {
      if (!this.longUrl.trim()) {
        this.error = 'Please enter a valid URL'
        return
      }

      if (!this.customAlias.trim()) {
        this.error = 'Please enter a custom alias'
        return
      }

      this.isLoading = true
      this.error = ''
      this.shortUrl = ''

      try {
        // Call your URL shortening API with custom alias through Ocelot Gateway
        const response = await fetch('http://localhost:8000/shorten', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            longUrl: this.longUrl,
            customAlias: this.customAlias
          })
        })

        if (!response.ok) {
          throw new Error('Failed to shorten URL')
        }

        await response.json() // Consume the response
        this.shortUrl = `${this.systemDomain}/${this.customAlias}`

      } catch (error) {
        console.error('Error shortening URL:', error)
        this.error = 'Failed to shorten URL. Please try again or choose a different alias.'
      } finally {
        this.isLoading = false
      }
    }
  }
}
</script>