import React, { useState, useEffect } from 'react'
import classes from 'classnames'
import './record.scss'

let startTime = 0
let chunks = []

const RecordButton = function (props) {
  const [ recorder, setRecorder ] = useState(null)
  const [ isRecording, setRecording] = useState(false)
  const { isRecognizing, isDisabled, onSend, small } = props 
  const [ time, setTime ] = useState(0)

  useEffect(function () {
    navigator.mediaDevices.getUserMedia({ audio: true})
      .then(stream => {
        const recorder = new MediaRecorder(stream)

        recorder.addEventListener('dataavailable', function(event) {
          chunks.push(event.data)
        })

        recorder.addEventListener('stop', function () {
          const audioBlob = new Blob(chunks, {
            type: 'audio/mp3'
          });
          
          onSend(audioBlob)
        })

        setRecorder(recorder)
      })
  }, [])

  const handleVoiceRecording = function (e) {
    if (recorder) {
      if (isRecording) {
        recorder.stop()
      } else {
        chunks = []
        recorder.start()
      }

      setRecording(!isRecording)
    }
  }
  
  const requestTime = function () {
    const time = Date.now() - startTime
    setTime(time)
    isRecording && !isDisabled && window.requestAnimationFrame(requestTime)
  }
  
  const formatTime = function (time) {
    const asSeconds = Math.floor(time / 1000)
    const asMinutes = Math.floor(asSeconds / 60)
    const hours = Math.floor(asMinutes / 60)
    const minutes = asMinutes - hours * 60
    const seconds = asSeconds - asMinutes * 60
    const milliseconds = Math.floor((time % 1000) / 100)
    
    return `${hours}:${('0'+minutes).substr(-2)}:${('0'+seconds).substr(-2)}.${milliseconds}`
  }
  
  useEffect(function () {
    if (isRecording) {
      startTime = Date.now()
      window.requestAnimationFrame(requestTime)
    }
  }, [isRecording])
  
  return <button 
    type='button' 
    className={classes('record', isDisabled && '-disabled', small && '-small')} 
    onClick={handleVoiceRecording}
  >
    <span className={classes('record-button', isRecording && '-stop')} />
    <span className='record-time'>
      { isRecognizing ? 'Распознавание...' : isRecording  ? formatTime(time) : <strong>Запись голоса</strong>}
    </span>
  </button>
}

export default RecordButton