import React, { useState, useEffect, useRef } from 'react'
import classes from 'classnames'
import { Loader } from '../../shared'
import { fetchAudio } from '../../services/task'
import './play-audio.scss'


const PlayAudio = function (props) {
  const { guid } = props
  const [isPlaying, setPlaying ] = useState(false)
  const [ audioBlob, setAudioBlob ] = useState(false)
  const audioRef = useRef(null)

  useEffect(function () {
    async function loadAudio (guid) {
      try {
        const blob = await fetchAudio(guid)
        setAudioBlob(URL.createObjectURL(blob))
      } catch {
        setAudioBlob(null)
      }
    }

    guid && loadAudio(guid)
  }, [ guid ])

  const handleClick = function () {
    if (audioRef.current) {
      if (!isPlaying) {
        audioRef.current.play()
        audioRef.current.addEventListener('ended', () => {
          setPlaying(false)
        })
      } else {
        audioRef.current.pause()
      }
      setPlaying(!isPlaying)
    }
  }
  
  return <div className='play-audio'>
    {audioBlob
      ? <button
        type='button'
        className={classes('play-audio', isPlaying && '-playing')}
        onClick={handleClick}
      >
        {!isPlaying ? 'Прослушать' : 'Остановить'}
      </button>
      : audioBlob === false ? <Loader/> : null
    }
    { audioBlob && <audio src={audioBlob} ref={audioRef} /> }
  </div>
}

export default PlayAudio