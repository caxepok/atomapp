import React from 'react'
import LayoutSection from '../../components/Layout/LayoutSection'
import { PageMessage } from '../../components/Page'

const Demo = function () {
  return <LayoutSection>
    <PageMessage>
      <div style={{
        margin: '0 -130px 0 -130px',
        width: 560,
        height: 315,
        background: '#000'
      }}>
        <iframe src="https://player.vimeo.com/video/484936329" width="640" height="467" frameborder="0" allow="autoplay; fullscreen" allowfullscreen></iframe>
      </div>
    </PageMessage>
  </LayoutSection>
}

export default Demo