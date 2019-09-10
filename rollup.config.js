import babel from 'rollup-plugin-babel'

export default {
  input: 'client/myapp.js',
  output: {
    file: 'client/index.js',
    format: 'iife'
  },
  plugins: [
    babel({
      exclude: 'node_modules/**' // only transpile our source code
    })
  ]
}