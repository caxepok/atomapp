import React from 'react'
import './textbox.scss'

const Textbox = function (props) {
  return <input type='text' className='textbox' {...props} />
}

export default Textbox