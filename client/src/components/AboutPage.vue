<template>
  <div class="min-h-screen bg-gray-100 py-12">
    <div class="max-w-4xl mx-auto px-4">
      <div class="text-center mb-12">
        <h1 class="text-4xl font-bold text-blue-600 mb-4">About ShortLink</h1>
        <p class="text-xl text-gray-700">The fastest and most reliable URL shortening service</p>
      </div>

      <div class="bg-white rounded-lg shadow-lg p-8 mb-8">
        <h2 class="text-2xl font-semibold text-gray-800 mb-4">Our Mission</h2>
        <p class="text-gray-600 leading-relaxed mb-6">
          We're dedicated to providing the fastest, most reliable URL shortening service on the web.
          Our platform helps businesses, marketers, and individuals create short, memorable links that
          are perfect for social media, email campaigns, and any situation where long URLs are impractical.
        </p>
        
        <h2 class="text-2xl font-semibold text-gray-800 mb-4">Our Technology Stack</h2>
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div class="flex items-center">
            <div class="bg-green-100 p-2 rounded-full mr-3">
              <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd"></path>
              </svg>
            </div>
            <span class="text-gray-700">Vue.js 3 - Modern Frontend Framework</span>
          </div>

          <div class="flex items-center">
            <div class="bg-green-100 p-2 rounded-full mr-3">
              <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd"></path>
              </svg>
            </div>
            <span class="text-gray-700">ASP.NET Core - High-Performance Backend</span>
          </div>

          <div class="flex items-center">
            <div class="bg-green-100 p-2 rounded-full mr-3">
              <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd"></path>
              </svg>
            </div>
            <span class="text-gray-700">Redis - Lightning-Fast Caching</span>
          </div>

          <div class="flex items-center">
            <div class="bg-green-100 p-2 rounded-full mr-3">
              <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd"></path>
              </svg>
            </div>
            <span class="text-gray-700">PostgreSQL - Reliable Database</span>
          </div>

          <div class="flex items-center">
            <div class="bg-green-100 p-2 rounded-full mr-3">
              <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd"></path>
              </svg>
            </div>
            <span class="text-gray-700">RabbitMQ - Event-Driven Architecture</span>
          </div>

          <div class="flex items-center">
            <div class="bg-green-100 p-2 rounded-full mr-3">
              <svg class="w-6 h-6 text-green-600" fill="currentColor" viewBox="0 0 20 20">
                <path fill-rule="evenodd" d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z" clip-rule="evenodd"></path>
              </svg>
            </div>
            <span class="text-gray-700">Ocelot Gateway - API Management</span>
          </div>
        </div>

        <div class="mt-8">
          <h3 class="text-xl font-semibold text-gray-800 mb-4">Key Features</h3>
          <ul class="space-y-3 text-gray-600">
            <li class="flex items-start">
              <span class="text-blue-600 mr-2">•</span>
              <span><strong>Microservices Architecture:</strong> Scalable and maintainable service-oriented design</span>
            </li>
            <li class="flex items-start">
              <span class="text-blue-600 mr-2">•</span>
              <span><strong>High-Performance Caching:</strong> Redis integration for sub-100ms response times</span>
            </li>
            <li class="flex items-start">
              <span class="text-blue-600 mr-2">•</span>
              <span><strong>Event-Driven Updates:</strong> Real-time synchronization across services</span>
            </li>
            <li class="flex items-start">
              <span class="text-blue-600 mr-2">•</span>
              <span><strong>Enterprise Security:</strong> Secure URL generation and validation</span>
            </li>
          </ul>
        </div>
      </div>

      <div class="text-center">
        <RouterLink 
          to="/" 
          class="bg-blue-600 text-white hover:bg-blue-700 px-6 py-3 rounded-lg font-medium transition-colors duration-200 inline-block"
        >
          Back to Home
        </RouterLink>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'AboutPage'
}
</script>
