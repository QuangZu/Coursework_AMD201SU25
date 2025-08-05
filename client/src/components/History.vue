<template>
  <div class="min-h-screen bg-gradient-to-br from-blue-50 to-indigo-100">
    <!-- Header Section -->
    <div class="bg-white shadow-lg">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div class="text-center">
          <h1 class="text-4xl font-bold text-gray-800 mb-4">
            Your URL <span class="text-blue-600">History</span>
          </h1>
          <p class="text-gray-600 text-lg">
            Track all your shortened URLs in one place
          </p>
        </div>
      </div>
    </div>

    <!-- Main Content -->
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
      <!-- Loading State -->
      <div v-if="isLoading" class="flex justify-center items-center py-20">
        <div class="text-center">
          <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p class="text-gray-600">Loading your history...</p>
        </div>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="bg-red-50 border border-red-200 rounded-xl p-6 mb-8">
        <div class="flex items-center">
          <svg class="h-6 w-6 text-red-600 mr-3" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.964-.833-2.732 0L3.732 16.5c-.77.833.192 2.5 1.732 2.5z" />
          </svg>
          <h3 class="text-lg font-semibold text-red-800">Error Loading History</h3>
        </div>
        <p class="text-red-600 mt-2">{{ error }}</p>
        <button 
          @click="loadHistory" 
          class="mt-4 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition-colors"
        >
          Try Again
        </button>
      </div>

      <!-- Empty State -->
      <div v-else-if="!history || history.length === 0" class="text-center py-20">
        <div class="max-w-md mx-auto">
          <div class="w-24 h-24 bg-gray-200 rounded-full flex items-center justify-center mx-auto mb-6">
            <svg class="h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
          </div>
          <h3 class="text-2xl font-bold text-gray-800 mb-4">No History Yet</h3>
          <p class="text-gray-600 mb-8">
            You haven't shortened any URLs yet. Start by creating your first short link!
          </p>
          <RouterLink 
            to="/" 
            class="inline-flex items-center px-6 py-3 bg-blue-600 text-white font-semibold rounded-xl hover:bg-blue-700 transition-colors"
          >
            <svg class="h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
            </svg>
            Create Short URL
          </RouterLink>
        </div>
      </div>

      <!-- History List -->
      <div v-else class="space-y-6">
        <!-- Stats -->
        <div class="bg-white rounded-2xl shadow-xl p-6 mb-8">
          <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div class="text-center">
              <div class="text-3xl font-bold text-blue-600">{{ history.length }}</div>
              <div class="text-gray-600">Total URLs</div>
            </div>
            <div class="text-center">
              <div class="text-3xl font-bold text-green-600">{{ recentCount }}</div>
              <div class="text-gray-600">This Week</div>
            </div>
            <div class="text-center">
              <div class="text-3xl font-bold text-purple-600">{{ user.username }}</div>
              <div class="text-gray-600">Your Account</div>
            </div>
          </div>
        </div>

        <!-- History Items -->
        <div class="space-y-4">
          <div 
            v-for="(item, index) in history" 
            :key="index"
            class="bg-white rounded-2xl shadow-lg p-6 hover:shadow-xl transition-shadow duration-200"
          >
            <div class="flex flex-col lg:flex-row lg:items-center justify-between gap-4">
              <!-- URL Information -->
              <div class="flex-1 min-w-0">
                <div class="flex items-start gap-4">
                  <div class="flex-shrink-0">
                    <div class="w-12 h-12 bg-blue-100 rounded-xl flex items-center justify-center">
                      <svg class="h-6 w-6 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13.828 10.172a4 4 0 00-5.656 0l-4 4a4 4 0 105.656 5.656l1.102-1.101m-.758-4.899a4 4 0 005.656 0l4-4a4 4 0 00-5.656-5.656l-1.1 1.1" />
                      </svg>
                    </div>
                  </div>
                  
                  <div class="flex-1 min-w-0">
                    <div class="mb-2">
                      <h3 class="text-lg font-semibold text-gray-800 mb-1">
                        Short URL
                      </h3>
                      <div class="flex items-center gap-2">
                        <a 
                          :href="getFullShortUrl(item.short_url)" 
                          target="_blank"
                          class="text-blue-600 hover:text-blue-800 font-medium break-all"
                        >
                          {{ getFullShortUrl(item.short_url) }}
                        </a>
                        <button 
                          @click="copyToClipboard(getFullShortUrl(item.short_url))"
                          class="text-gray-400 hover:text-blue-600 transition-colors"
                          :title="copiedIndex === index ? 'Copied!' : 'Copy URL'"
                        >
                          <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" />
                          </svg>
                        </button>
                      </div>
                    </div>
                    
                    <div>
                      <h4 class="text-sm font-medium text-gray-600 mb-1">Original URL</h4>
                      <p class="text-gray-800 break-all text-sm">
                        {{ item.long_url }}
                      </p>
                    </div>
                  </div>
                </div>
              </div>

              <!-- Actions and Date -->
              <div class="flex flex-col items-end gap-3">
                <div class="text-right">
                  <div class="text-sm text-gray-500">
                    {{ formatDate(item.created_at) }}
                  </div>
                  <div class="text-xs text-gray-400">
                    {{ formatTimeAgo(item.created_at) }}
                  </div>
                </div>
                
                <div class="flex gap-2">
                  <button 
                    @click="visitUrl(getFullShortUrl(item.short_url))"
                    class="px-4 py-2 bg-blue-600 text-white text-sm rounded-lg hover:bg-blue-700 transition-colors"
                  >
                    Visit
                  </button>
                  <button 
                    @click="deleteHistoryItem(index)"
                    class="px-4 py-2 bg-red-100 text-red-600 text-sm rounded-lg hover:bg-red-200 transition-colors"
                  >
                    Delete
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { authAPI } from '../utils/api'

export default {
  name: 'HistoryPage',
  data() {
    return {
      isLoading: true,
      error: '',
      history: [],
      user: null,
      copiedIndex: null
    }
  },
  computed: {
    recentCount() {
      if (!this.history) return 0
      const oneWeekAgo = new Date()
      oneWeekAgo.setDate(oneWeekAgo.getDate() - 7)
      return this.history.filter(item => new Date(item.created_at) > oneWeekAgo).length
    }
  },
  async mounted() {
    await this.loadHistory()
  },
  methods: {
    async loadHistory() {
      this.isLoading = true
      this.error = ''
      
      try {
        const token = localStorage.getItem('token')
        if (!token) {
          this.$router.push('/login')
          return
        }

        const response = await authAPI.getProfile()
        this.user = response.data.user
        this.history = response.data.history || []
        
        // Emit user data to parent
        this.$emit('user-loaded', this.user)
        
      } catch (error) {
        console.error('Error loading history:', error)
        this.error = 'Failed to load your history. Please try again.'
      } finally {
        this.isLoading = false
      }
    },

    getFullShortUrl(shortUrl) {
      return `http://localhost:8000/${shortUrl}`
    },

    copyToClipboard(text) {
      navigator.clipboard.writeText(text).then(() => {
        this.copiedIndex = this.history.findIndex(item => 
          this.getFullShortUrl(item.short_url) === text
        )
        setTimeout(() => {
          this.copiedIndex = null
        }, 2000)
      }).catch(err => {
        console.error('Failed to copy:', err)
      })
    },

    visitUrl(url) {
      window.open(url, '_blank')
    },

    deleteHistoryItem(index) {
      // Note: Backend doesn't have delete history endpoint yet
      // This is a placeholder for future implementation
      if (confirm('Are you sure you want to delete this URL from your history?')) {
        this.history.splice(index, 1)
        // Remove item from local array (backend deletion not implemented yet)
      }
    },

    formatDate(dateString) {
      const date = new Date(dateString)
      return date.toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      })
    },

    formatTimeAgo(dateString) {
      const date = new Date(dateString)
      const now = new Date()
      const diffInSeconds = Math.floor((now - date) / 1000)
      
      if (diffInSeconds < 60) {
        return 'Just now'
      } else if (diffInSeconds < 3600) {
        const minutes = Math.floor(diffInSeconds / 60)
        return `${minutes} minute${minutes > 1 ? 's' : ''} ago`
      } else if (diffInSeconds < 86400) {
        const hours = Math.floor(diffInSeconds / 3600)
        return `${hours} hour${hours > 1 ? 's' : ''} ago`
      } else {
        const days = Math.floor(diffInSeconds / 86400)
        return `${days} day${days > 1 ? 's' : ''} ago`
      }
    }
  }
}
</script>