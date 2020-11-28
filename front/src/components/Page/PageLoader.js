import React from 'react'
import { Loader } from '../../shared'
import './page.scss'

const PageLoader = function () {
  return <div className='page-loader'>
    <h1><Loader /></h1>
  </div>
}

export default PageLoader