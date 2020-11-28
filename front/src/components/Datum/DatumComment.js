import React from 'react'
import { getFormattedDate } from '../../shared/helpers'
import './datum.scss'

const DatumComment = function (props) {
  const { creatorName, addedAt, text } = props 
  
  return <dd className='datum-comment'>
    {creatorName && <span className='datum-comment-person'>{creatorName}</span> }
    {addedAt && <span className='datum-comment-date'>{getFormattedDate(addedAt)}</span>}
    <span className='datum-comment-text'>{text}</span>
  </dd>
}

export default DatumComment