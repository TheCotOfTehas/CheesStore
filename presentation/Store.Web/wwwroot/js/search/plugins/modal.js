$.modal = function (options) {
    const $modal = _createModal(options)
    const DEFAULT_WIDTH = '600px'
    const ANIMATION_SPEED = 200
    let closing = false

    return {
            open() {
                !closing && $modal.classList.add('open')
            },
        close() {
                closing = true
                $modal.classList.remove('open')
                $modal.classList.add('hide')
                setTimeout(() => {
                    $modal.classList.remove('hide')
                    closing = false
                }, ANIMATION_SPEED)
            }
        }
    }

function _createModal(options) {
    const DEFAULT_WIDTH = '600px'
    const modal = document.createElement( 'div')
    modal.classList.add('myModal')
    modal.insertAdjacentHTML('afterbegin', `
        <div class="modal-overlay">
            <div class="modal-window">
                    <div class="modal-header">
                        <span class="modal-title">modal-title</span>
                        <span class="modal-close" data-close="true">&times;</span>
                    </div>
                    <div class="modal-body"  data-Content>
                        <p>modal-body</p>
                    </div>
                    <div class="modal-footer">
                        <button>Ok</button>
                        <button>Cancel</button>
                    </div>
            </div>
        </div>
    `)

    document.body.appendChild(modal)
    return modal
}